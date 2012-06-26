using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.TransientFaultHandling;
using ApiBridge.Bus.Interfaces;
using Autofac;
using System.Reflection;
using ApiBridge.Bus.Utils;
using ApiBridge.Bus.Container;

namespace ApiBridge.Bus.Core
{
    public class BusConfigurationBuilder
    {
        BusConfiguration configuration;

        internal BusConfigurationBuilder(BusConfiguration configuration)
        {
            Guard.ArgumentNotNull(configuration, "configuration");
            
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the bus configuration associated with the builder
        /// </summary>
        public IBusConfiguration Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// Mark the configuration as complete
        /// </summary>
        public void Configure()
        {
            Configuration.Configure();
        }

        /// <summary>
        /// Auto discover all of the Subscribers in the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public BusConfigurationBuilder RegisterAssembly(Assembly assembly)
        {
            Guard.ArgumentNotNull(assembly, "assembly");
            configuration.AddRegisteredAssembly(assembly);
            return this;
        }

        /// <summary>
        /// Register just one subscriber.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BusConfigurationBuilder RegisterSubscriber<T>()
        {
            configuration.AddRegisteredSubscriber(typeof(T));
            return this;
        }

        /// <summary>
        /// Set the ServiceBusApplicationId
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder ServiceBusApplicationId(string value)
        {
            Guard.ArgumentNotNullOrEmptyString(value, "value");
            if (value.Length > 10)
            {
                throw new ArgumentOutOfRangeException("The length must not be greater than 10.");
            }
            configuration.ServiceBusApplicationId = value;
            return this;
        }

        /// <summary>
        /// Set the DefaultSerializer
        /// </summary>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public BusConfigurationBuilder DefaultSerializer(IServiceBusSerializer serializer)
        {
            //configuration.Container.Register(typeof(IServiceBusSerializer), serializer.GetType());
            return this;
        }

        /// <summary>
        /// Set the Max Threads that will pull from the bus.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder MaxThreads(byte value)
        {
            Guard.ArgumentNotZeroOrNegativeValue(value, "value");
            configuration.MaxThreads = value;
            return this;
        }

        /// <summary>
        /// Read the ServiceBus Application and Service Bus Settings from the config file
        /// </summary>
        /// <returns></returns>
        public BusConfigurationBuilder ReadFromConfigFile()
        {
            var setting = BusConfigurationHelper.GetConfig("ServiceBusApplicationId");
            configuration.ServiceBusApplicationId = setting;

            setting = BusConfigurationHelper.GetConfig("ServiceBusIssuerKey");
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new ArgumentNullException("ServiceBusIssuerKey", "The ServiceBusIssuerKey must be set.");
            }
            configuration.ServiceBusIssuerKey = setting;

            setting = BusConfigurationHelper.GetConfig("ServiceBusIssuerName");
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new ArgumentNullException("ServiceBusIssuerName", "The ServiceBusIssuerName must be set.");
            }
            configuration.ServiceBusIssuerName = setting;

            setting = BusConfigurationHelper.GetConfig("ServiceBusNamespace");
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new ArgumentNullException("ServiceBusNamespace", "The ServiceBusNamespace must be set.");
            }
            configuration.ServiceBusNamespace = setting;

            return this;
        }

        /// <summary>
        /// ServiceBusNamespace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder ServiceBusNamespace(string value)
        {
            Guard.ArgumentNotNull(value, "value");
            configuration.ServiceBusNamespace = value;
            return this;
        }

        /// <summary>
        /// ServiceBusIssuerName
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder ServiceBusIssuerName(string value)
        {
            Guard.ArgumentNotNull(value, "value");
            configuration.ServiceBusIssuerName = value;
            return this;
        }

        /// <summary>
        /// ServiceBusIssuerKey
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder ServiceBusIssuerKey(string value)
        {
            Guard.ArgumentNotNull(value, "value");
            configuration.ServiceBusIssuerKey = value;
            return this;
        }

        /// <summary>
        /// ServicePath
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder ServicePath(string value)
        {
            Guard.ArgumentNotNull(value, "value");
            configuration.ServicePath = value;
            return this;
        }

        /// <summary>
        /// Override the default TopicName
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder TopicName(string value)
        {
            Guard.ArgumentNotNull(value, "value");
            configuration.TopicName = value;
            return this;
        }

        /// <summary>
        /// Override the default OutboundTopicName
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder OutboundTopicName(string value)
        {
            Guard.ArgumentNotNull(value, "value");
            configuration.OutboundTopicName = value;
            return this;
        }

        /// <summary>
        /// Override the default OutboundTopicName
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BusConfigurationBuilder InboundTopicName(string value)
        {
            Guard.ArgumentNotNull(value, "value");
            configuration.InboundTopicName = value;
            return this;
        }

        /// <summary>
        /// Initializes Autofac
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="container">Autofac container used in your application.  This is optional.  A new container will be created if one is not provided</param>
        /// <returns></returns>
        public BusConfigurationBuilder UseAutofacContainer(IContainer container = null)
        {
            configuration.Container = new AutofacBusContainer(container);
            return this;
        }
    }
}
