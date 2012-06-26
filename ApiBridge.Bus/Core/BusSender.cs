using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Bus.Interfaces;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using System.Threading;

namespace ApiBridge.Bus.Core
{
    class BusSender : SenderBase, IBusSender
    {
        TopicClient client;

        public BusSender(IBusConfiguration configuration) : base(configuration) {
            Guard.ArgumentNotNull(configuration, "configuration");
            retryPolicy.ExecuteAction(() => {
                client = configurationFactory.MessageFactory.CreateTopicClient(topic.Path);
            });
        }

        public void Close() {
            if (client != null) {
                client.Close();
                client = null;
            }
        }

        public void Send<T>(T obj) {
            Send<T>(obj, null);
        }

        public void Send<T>(T obj, IDictionary<string, object> metadata) {
            Send<T>(obj, configuration.DefaultSerializer.Create(), metadata);
        }

        public void Send<T>(T obj, IServiceBusSerializer serializer = null, IDictionary<string, object> metadata = null) {
            Guard.ArgumentNotNull(obj, "obj");

            // Declare a wait object that will be used for synchronization.
            var waitObject = new ManualResetEvent(false);

            // Declare a timeout value during which the messages are expected to be sent.
            var sentTimeout = TimeSpan.FromSeconds(30);

            Exception failureException = null;

            SendAsync<T>(obj, null, (result) => {
                waitObject.Set();
                failureException = result.ThrownException;
            }, metadata);

            // Wait until the messaging operations are completed.
            bool completed = waitObject.WaitOne(sentTimeout);
            waitObject.Dispose();

            if (failureException != null) {
                throw failureException;
            }

            if (!completed) {
                throw new Exception("Failed to Send Message. Reason was timeout.");
            }
        }

        public void SendAsync<T>(T obj, object state, Action<ICommandSent<T>> resultCallBack)
        {
            SendAsync<T>(obj, state, resultCallBack, configuration.DefaultSerializer.Create());
        }

        public void SendAsync<T>(T obj, object state, Action<ICommandSent<T>> resultCallBack, IDictionary<string, object> metadata)
        {
            SendAsync<T>(obj, state, resultCallBack, configuration.DefaultSerializer.Create(), metadata);
        }

        public void SendAsync<T>(T methodObj, object methodState, Action<ICommandSent<T>> methodResultCallBack,
            IServiceBusSerializer methodSerializer = null, IDictionary<string, object> methodMetadata = null) 
        {

            Guard.ArgumentNotNull(methodObj, "obj");
            Guard.ArgumentNotNull(methodResultCallBack, "resultCallBack");

            methodSerializer = methodSerializer ?? configuration.DefaultSerializer.Create();

            Action<T, object, Action<ICommandSent<T>>, IServiceBusSerializer, IDictionary<string, object>> sendAction = null;

            sendAction = ((obj, state, resultCallBack, serializer, metadata) => {

                BrokeredMessage message = null;
                Exception failureException = null;
                bool resultSent = false; //I am not able to determine when the exception block is called.

                // Use a retry policy to execute the Send action in an asynchronous and reliable fashion.
                retryPolicy.ExecuteAction
                (
                    (cb) => {
                        failureException = null; //we may retry so we must null out the error.
                        try {
                            // A new BrokeredMessage instance must be created each time we send it. Reusing the original BrokeredMessage instance may not 
                            // work as the state of its BodyStream cannot be guaranteed to be readable from the beginning.
                            message = new BrokeredMessage(serializer.Serialize(obj), false);

                            // string based serializer
                            //message = new BrokeredMessage(serializer.SerializeJson(obj));

                            message.MessageId = Guid.NewGuid().ToString();
                            message.Properties.Add(TYPE_HEADER_NAME, obj.GetType().FullName.Replace('.', '_'));

                            if (metadata != null) {
                                foreach (var item in metadata) {
                                    message.Properties.Add(item.Key, item.Value);
                                }
                            }

                            // Send the event asynchronously.
                            client.BeginSend(message, cb, null);
                        }
                        catch (Exception ex) {
                            failureException = ex;
                            throw;
                        }
                    },
                    (ar) => {
                        try {
                            failureException = null; //we may retry so we must null out the error.
                            client.EndSend(ar);
                        }
                        catch (Exception ex) {
                            failureException = ex;
                            throw;
                        }
                    },
                    () => {
                        // Ensure that any resources allocated by a BrokeredMessage instance are released.
                        if (message != null) {
                            message.Dispose();
                            message = null;
                        }
                        if (serializer != null) {
                            serializer.Dispose();
                            serializer = null;
                        }
                        if (!resultSent) {
                            resultSent = true;
                            ExtensionMethods.ExecuteAndReturn(() => resultCallBack(new CommandSent<T>()
                            {
                                IsSuccess = failureException == null,
                                State = state,
                                ThrownException = failureException
                            }));
                        }
                    },
                    (ex) => {
                        // Always dispose the BrokeredMessage instance even if the send operation has completed unsuccessfully.
                        if (message != null) {
                            message.Dispose();
                            message = null;
                        }
                        if (serializer != null) {
                            serializer.Dispose();
                            serializer = null;
                        }
                        failureException = ex;

                        // Always process exceptions.
                        if (!resultSent) {
                            resultSent = true;
                            ExtensionMethods.ExecuteAndReturn(() => resultCallBack(new CommandSent<T>()
                            {
                                IsSuccess = failureException == null,
                                State = state,
                                ThrownException = failureException
                            }));
                        }
                    }
                ); //asyc
            }); //action

            sendAction(methodObj, methodState, methodResultCallBack, methodSerializer, methodMetadata);
        }

        public override void Dispose(bool disposing) {
            Close();
        }

    }
}
