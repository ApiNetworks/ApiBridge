using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Bus.Interfaces;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;
using System.Threading;

namespace ApiBridge.Bus.Core
{
    class BusReceiver : ReceiverBase, IBusReceiver
    {
        object lockObject = new object();

        List<ReceiverHelper> mappings = new List<ReceiverHelper>();

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration">The configuration data</param>
        public BusReceiver(IBusConfiguration configuration)
            : base(configuration)
        {
            Guard.ArgumentNotNull(configuration, "configuration");
        }

        /// <summary>
        /// Create a new Subscription.
        /// </summary>
        /// <param name="value">The data used to create the subscription</param>
        public void CreateSubscription(ServiceBusEnpointData value)
        {
            Guard.ArgumentNotNull(value, "value");

            //TODO determine how we can change the filters for an existing registered item
            //ServiceBusNamespaceClient

            lock (lockObject)
            {
                SubscriptionDescription desc = null;

                bool createNew = false;

                try
                {
                    // First, let's see if a item with the specified name already exists.
                    verifyRetryPolicy.ExecuteAction(() =>
                    {
                        desc = configurationFactory.NamespaceManager.GetSubscription(topic.Path, value.SubscriptionName);
                    });

                    createNew = (desc == null);
                }
                catch (MessagingEntityNotFoundException)
                {
                    // Looks like the item does not exist. We should create a new one.
                    createNew = true;
                }

                // If a item with the specified name doesn't exist, it will be auto-created.
                if (createNew)
                {
                    var descriptionToCreate = new SubscriptionDescription(topic.Path, value.SubscriptionName);

                    if (value.AttributeData != null)
                    {
                        var attr = value.AttributeData;
                        if (attr.DefaultMessageTimeToLiveSet())
                        {
                            descriptionToCreate.DefaultMessageTimeToLive = new TimeSpan(0, 0, attr.DefaultMessageTimeToLive);
                        }
                        descriptionToCreate.EnableBatchedOperations = attr.EnableBatchedOperations;
                        descriptionToCreate.EnableDeadLetteringOnMessageExpiration = attr.EnableDeadLetteringOnMessageExpiration;
                        if (attr.LockDurationSet())
                        {
                            descriptionToCreate.LockDuration = new TimeSpan(0, 0, attr.LockDuration);
                        }
                    }

                    try
                    {
                        var filter = new SqlFilter(string.Format(TYPE_HEADER_NAME + " = '{0}'", value.MessageType.FullName.Replace('.', '_')));
                        retryPolicy.ExecuteAction(() =>
                        {
                            desc = configurationFactory.NamespaceManager.CreateSubscription(descriptionToCreate, filter);
                        });
                    }
                    catch (MessagingEntityAlreadyExistsException)
                    {
                        // A item under the same name was already created by someone else, perhaps by another instance. Let's just use it.
                        retryPolicy.ExecuteAction(() =>
                        {
                            desc = configurationFactory.NamespaceManager.GetSubscription(topic.Path, value.SubscriptionName);
                        });
                    }
                }

                SubscriptionClient subscriptionClient = null;
                var rm = ReceiveMode.PeekLock;

                if (value.AttributeData != null)
                {
                    rm = value.AttributeData.ReceiveMode;
                }

                retryPolicy.ExecuteAction(() =>
                {
                    subscriptionClient = configurationFactory.MessageFactory.CreateSubscriptionClient(topic.Path, value.SubscriptionName, rm);
                });

                if (value.AttributeData != null && value.AttributeData.PrefetchCountSet())
                {
                    subscriptionClient.PrefetchCount = value.AttributeData.PrefetchCount;
                }

                BusReceiverState state = new BusReceiverState();
                state.Client = subscriptionClient;
                state.EndPointData = value;

                var helper = new ReceiverHelper(configuration, retryPolicy, state);
                mappings.Add(helper);
                helper.ProcessMessagesForSubscription();

            } //lock end

        }

        /// <summary>
        /// Cancel a subscription
        /// </summary>
        /// <param name="value">The data used to cancel the subscription</param>
        public void CancelSubscription(ServiceBusEnpointData value)
        {
            Guard.ArgumentNotNull(value, "value");
            var subscription = mappings.FirstOrDefault(item => item.Data.EndPointData.SubscriptionName.Equals(value.SubscriptionName, StringComparison.OrdinalIgnoreCase));

            if (subscription == null)
            {
                return;
            }

            subscription.Data.Cancel();

            Task t = Task.Factory.StartNew(() =>
            {
                //HACK find better way to wait for a cancel request so we are not blocking.
                for (int i = 0; i < 100; i++)
                {
                    if (!subscription.Data.Cancelled)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                }

                if (configurationFactory.NamespaceManager.SubscriptionExists(topic.Path, value.SubscriptionName))
                {
                    var filter = new SqlFilter(string.Format(TYPE_HEADER_NAME + " = '{0}'", value.MessageType.FullName.Replace('.', '_')));
                    retryPolicy.ExecuteAction(() => configurationFactory.NamespaceManager.DeleteSubscription(topic.Path, value.SubscriptionName));
                }
            });

            try
            {
                Task.WaitAny(t);
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    //do nothing
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            foreach (var item in mappings)
            {
                item.Data.Client.Close();
                if (item is IDisposable)
                {
                    (item as IDisposable).Dispose();
                }
                item.Data.Client = null;
            }
            mappings.Clear();
        }

    }

}
