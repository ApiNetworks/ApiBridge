using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ApiBridge.Bus.Interfaces;
using Autofac;
using System.Reflection;
using Microsoft.Practices.TransientFaultHandling;
using ApiBridge.Bus.Factories;
using ApiBridge.Bus.Serialization;
using ApiBridge.Bus.Container;

namespace ApiBridge.Bus.Core
{
    public class BusConfiguration : IBusConfiguration
    {
        static readonly BusConfiguration configuration = new BusConfiguration();

        //private IContainer container;
        //private ContainerBuilder builder = new ContainerBuilder();

        IBusContainer container;

        List<Assembly> registeredAssemblies = new List<Assembly>();
        List<Type> registeredSubscribers = new List<Type>();

        static BusConfiguration()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        private BusConfiguration()
        {
            MaxThreads = 1;
            TopicName = "apibridgebus";
        }

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static IBusConfiguration Instance
        {
            get
            {
                return configuration; 
            }
        }

        public IBus Bus
        {
            get
            {
                return container.Resolve<IBus>();
            }
        }


        public IBusContainer Container
        {
            get
            {
                return container;
            }
            set
            {
                if (container != null)
                {
                    throw new NotSupportedException("The container can only be set once.");
                }
                if (value != null)
                {
                    container = value;
                }
                else
                {
                    throw new ArgumentNullException("Container can not be null.");
                }
            }
        }

        /// <summary>
        /// Get the settings builder optionally passing in your existing IOC Container
        /// </summary>
        /// <returns></returns>
        public static BusConfigurationBuilder WithSettings()
        {
            return new BusConfigurationBuilder(configuration);
        }

        /// <summary>
        /// Apply the configuration
        /// </summary>
        public void Configure()
        {
            if (string.IsNullOrWhiteSpace(ServiceBusApplicationId))
            {
                throw new ApplicationException("ApplicationId must be set.");
            }

            container.RegisterConfiguration();
            if (!container.IsRegistered(typeof(IBus)))
            {
                container.Register(typeof(IBus), typeof(Bus));
            }
            if (!container.IsRegistered(typeof(IBusReceiver)))
            {
                container.Register(typeof(IBusReceiver), typeof(BusReceiver));
            }
            if (!container.IsRegistered(typeof(IBusSender)))
            {
                container.Register(typeof(IBusSender), typeof(BusSender));
            }
            if (!container.IsRegistered(typeof(IServiceBusConfigurationFactory)))
            {
                container.Register(typeof(IServiceBusConfigurationFactory), typeof(ServiceBusConfigurationFactory));
            }
            if (!container.IsRegistered(typeof(INamespaceManager)))
            {
                container.Register(typeof(INamespaceManager), typeof(ServiceBusNamespaceManagerFactory));
            }
            if (!container.IsRegistered(typeof(IMessagingFactory)))
            {
                container.Register(typeof(IMessagingFactory), typeof(ServiceBusMessagingFactory));
            }
            if (!container.IsRegistered(typeof(IServiceBusSerializer)))
            {
                // NewtonJson
                //container.Register(typeof(IServiceBusSerializer), typeof(JsonServiceBusSerializer));
                container.Register(typeof(IServiceBusSerializer), typeof(FastJsonServiceBusSerializer));
            }
            container.Build();

            //Set the Bus property so that the receiver will register the end points
            var prime = this.Bus;
        }

        internal void AddRegisteredAssembly(Assembly value)
        {
            Guard.ArgumentNotNull(value, "value");

            if (!this.registeredAssemblies.Contains(value))
            {
                this.registeredAssemblies.Add(value);
            }
        }

        internal void AddRegisteredSubscriber(Type value)
        {
            Guard.ArgumentNotNull(value, "value");
            if (!this.registeredSubscribers.Contains(value))
            {
                this.registeredSubscribers.Add(value);
            }
        }

        /// <summary>
        /// List of RegisteredSubscribers
        /// </summary>
        public IList<Type> RegisteredSubscribers
        {
            get
            {
                return this.registeredSubscribers;
            }
        }

        /// <summary>
        /// List of RegisteredAssemblies
        /// </summary>
        public IList<Assembly> RegisteredAssemblies
        {
            get
            {
                return this.registeredAssemblies;
            }
        }

        public IServiceBusSerializer DefaultSerializer
        {
            get
            {
                return container.Resolve<IServiceBusSerializer>();
            }
        }

        public byte MaxThreads
        {
            get;
            internal set;
        }

        public string ServiceBusApplicationId
        {
            get;
            internal set;
        }

        public string ServiceBusIssuerKey
        {
            get;
            internal set;
        }

        public string ServiceBusIssuerName
        {
            get;
            internal set;
        }

        public string ServiceBusNamespace
        {
            get;
            internal set;
        }

        public string ServicePath
        {
            get;
            internal set;
        }

        public string TopicName
        {
            get;
            internal set;
        }

        public string OutboundTopicName
        {
            get;
            internal set;
        }

        public string InboundTopicName
        {
            get;
            internal set;
        }
    }
}
