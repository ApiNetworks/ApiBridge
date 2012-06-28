using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Bus.Interfaces;
using System.Web;

using Autofac;

using Microsoft.Practices.TransientFaultHandling;
using System.Reflection;
using ApiBridge.Bus.Utils;
using ApiBridge.Bus.Attributes;
using ApiBridge.Handlers.Interfaces;

namespace ApiBridge.Bus.Core
{
    /// <summary>
    /// Implementation of IBus
    /// </summary>
    public class Bus : IBus
    {
        IBusConfiguration config;
        IBusSender sender;
        IBusReceiver receiver;

        List<Type> subscribedTypes = new List<Type>();

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="config"></param>
        public Bus(IBusConfiguration config)
        {
            Guard.ArgumentNotNull(config, "config");
            this.config = config;
            Configure();
        }

        /// <summary>
        /// Auto discover all of the Subscribers in the assembly.
        /// </summary>
        /// <param name="assembly">The assembly to register</param>
        public void RegisterAssembly(Assembly assembly)
        {
            Guard.ArgumentNotNull(assembly, "assembly");

            foreach (var type in assembly.GetTypes())
            {
                var interfaces = type.GetInterfaces()
                                .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) || i.GetGenericTypeDefinition() == typeof(ICompetingCommandHandler<>)))
                                .ToList();
                if (interfaces.Count > 0)
                {
                    Subscribe(type);
                }
            }
        }

        /// <summary>
        /// Publish a Message with the given signature.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to publish.</param>
        public void Publish<T>(T message)
        {
            sender.Send<T>(message, default(IDictionary<string, object>));
        }

        /// <summary>
        /// Publish a Message with the given signature.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to publish.</param>
        /// <param name="metadata">Metadata to sent with the message.</param>
        public void Publish<T>(T message, IDictionary<string, object> metadata = null)
        {
            Guard.ArgumentNotNull(message, "message");
            sender.Send<T>(message, metadata);
        }

        /// <summary>
        /// Publish a Message with the given signature.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to publish.</param>
        /// <param name="resultCallBack">The callback when the operation completes</param>
        /// <param name="metadata">Metadata to sent with the message.</param>
        public void PublishAsync<T>(T message, Action<ICommandSent<T>> resultCallBack, IDictionary<string, object> metadata)
        {
            sender.SendAsync<T>(message, null, resultCallBack, metadata);
        }

        /// <summary>
        /// Publish a Message with the given signature.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message to publish.</param>
        /// <param name="state">State object that is returned to the user</param>
        /// <param name="resultCallBack">The callback when the operation completes</param>
        /// <param name="metadata">Metadata to sent with the message.</param>
        public void PublishAsync<T>(T message, object state, Action<ICommandSent<T>> resultCallBack, IDictionary<string, object> metadata)
        {
            Guard.ArgumentNotNull(message, "message");
            Guard.ArgumentNotNull(resultCallBack, "resultCallBack");
            sender.SendAsync<T>(message, state, resultCallBack, metadata);
        }

        /// <summary>
        /// Subscribes to recieve published messages of type T.
        /// This method is only necessary if you turned off auto-subscribe
        /// </summary>
        /// <typeparam name="T">The type of message to subscribe to.</typeparam>
        public void Subscribe<T>()
        {
            Subscribe(typeof(T));
        }

        /// <summary>
        /// Subscribes to recieve published messages of type T.
        /// This method is only necessary if you turned off auto-subscribe
        /// </summary>
        /// <typeparam name="T">The type of message to subscribe to.</typeparam>
        /// <param name="type">The type to subscribe</param>
        public void Subscribe(Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            subscribedTypes.Add(type);
            SubscribeOrUnsubscribeType(type, config, receiver.CreateSubscription);
        }

        /// <summary>
        /// Unsubscribes from receiving published messages of the specified type
        /// </summary>
        /// <typeparam name="T">The type of message to unsubscribe from</typeparam>
        public void Unsubscribe<T>()
        {
            Unsubscribe(typeof(T));
        }

        /// <summary>
        /// Unsubscribes from receiving published messages of the specified type.
        /// </summary>
        /// <param name="type">The type of message to unsubscribe from</param>
        public void Unsubscribe(Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            if (subscribedTypes.Contains(type))
            {
                subscribedTypes.Remove(type);
            }
            SubscribeOrUnsubscribeType(type, config, receiver.CancelSubscription);
        }

        void Configure()
        {
            //this fixes a bug in .net 4 that will be fixed in sp1
            using (CloudEnvironment.EnsureSafeHttpContext())
            {
                //set up the server first.
                // Only resolve sender when a topic name is specified
                if (!String.IsNullOrEmpty(config.OutboundTopicName))
                    sender = config.Container.Resolve<IBusSender>();

                // Only resolve receiver when an inbound topic name is specified
                if (!String.IsNullOrEmpty(config.InboundTopicName))
                {
                    receiver = config.Container.Resolve<IBusReceiver>();
                    foreach (var item in config.RegisteredAssemblies)
                    {
                        RegisterAssembly(item);
                    }

                    foreach (var item in config.RegisteredSubscribers)
                    {
                        Subscribe(item);
                    }
                }
            }
        }

        void RegisterAssembly(IEnumerable<Assembly> assemblies)
        {
            Guard.ArgumentNotNull(assemblies, "assemblies");
            foreach (var item in assemblies)
            {
                RegisterAssembly(item);
            }
        }

        internal static bool IsCompetingHandler(Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(ICompetingCommandHandler<>);
        }

        internal static void SubscribeOrUnsubscribeType(Type type, IBusConfiguration config, Action<ServiceBusEnpointData> callback)
        {
            Guard.ArgumentNotNull(type, "type");
            Guard.ArgumentNotNull(callback, "callback");

            var interfaces = type.GetInterfaces()
                            .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) || i.GetGenericTypeDefinition() == typeof(ICompetingCommandHandler<>)))
                            .ToList();

            if (interfaces.Count == 0)
            {
                throw new ApplicationException(string.Format("Type {0} does not implement IHandleMessages or IHandleCompetingMessages", type.FullName));
            }

            //for each interface we find, we need to register it with the bus.
            foreach (var foundInterface in interfaces)
            {

                var implementedMessageType = foundInterface.GetGenericArguments()[0];
                //due to the limits of 50 chars we will take the name and a MD5 for the name.
                var hashName = implementedMessageType.FullName + "|" + type.FullName;

                var hash = MD5Helper.CalculateMD5(hashName);
                var fullName = (IsCompetingHandler(foundInterface) ? "C_" : config.ServiceBusApplicationId + "_") + hash;

                var info = new ServiceBusEnpointData()
                {
                    AttributeData = type.GetCustomAttributes(typeof(MessageHandlerConfigurationAttribute), false).FirstOrDefault() as MessageHandlerConfigurationAttribute,
                    DeclaredType = type,
                    MessageType = implementedMessageType,
                    SubscriptionName = fullName,
                    ServiceType = foundInterface
                };

                if (!config.Container.IsRegistered(type))
                {
                    if (info.IsReusable)
                    {
                        config.Container.Register(type, type);
                    }
                    else
                    {
                        config.Container.Register(type, type, true);
                    }
                }

                callback(info);
            }

            config.Container.Build();
        }
    }


    public static class CloudEnvironment {
    
        /// <summary>
        /// Ensures that the HttpContext object is safe to use in the given context and returns an object that rolls the HttpContext object back to its original state.
        /// </summary>
        /// <returns>An object that needs to be explicitly disposed so that HttpContext can return back to its original state.</returns>
        public static IDisposable EnsureSafeHttpContext() {
            HttpContext oldHttpContext = HttpContext.Current;
            HttpContext.Current = null;

            return new AnonymousDisposable(() => {
                HttpContext.Current = oldHttpContext;
            });
        }
    }
}
