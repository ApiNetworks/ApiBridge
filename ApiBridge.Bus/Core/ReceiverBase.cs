using System;
using ApiBridge.Bus.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using Autofac;

namespace ApiBridge.Bus.Core
{
    /// <summary>
    /// Base class for the sender and receiver client.
    /// </summary>
    abstract class ReceiverBase : IDisposable
    {
        internal static string TYPE_HEADER_NAME = "x_proj_ext_type"; //- are not allowed if you filter.
        protected IBusConfiguration configuration;
        protected IServiceBusConfigurationFactory configurationFactory;

        protected RetryPolicy<ServiceBusTransientErrorDetectionStrategy> retryPolicy
            = new RetryPolicy<ServiceBusTransientErrorDetectionStrategy>(20, RetryStrategy.DefaultMinBackoff, TimeSpan.FromSeconds(5.0), RetryStrategy.DefaultClientBackoff);
        protected RetryPolicy<ServiceBusTransientErrorToDetermineExistanceDetectionStrategy> verifyRetryPolicy
            = new RetryPolicy<ServiceBusTransientErrorToDetermineExistanceDetectionStrategy>(5, RetryStrategy.DefaultMinBackoff, TimeSpan.FromSeconds(2.0), RetryStrategy.DefaultClientBackoff);
        protected TopicDescription topic;

        /// <summary>
        /// Base class used to send and receive messages.
        /// </summary>
        /// <param name="configuration"></param>
        public ReceiverBase(IBusConfiguration configuration)
        {
            Guard.ArgumentNotNull(configuration, "configuration");
            this.configuration = configuration;

            configurationFactory = configuration.Container.Resolve<IServiceBusConfigurationFactory>();
            EnsureTopic(configuration.InboundTopicName);
        }

        protected void EnsureTopic(string topicName)
        {
            Guard.ArgumentNotNull(topicName, "topicName");
            bool createNew = false;

            try
            {
                // First, let's see if a topic with the specified name already exists.
                topic = verifyRetryPolicy.ExecuteAction<TopicDescription>(() =>
                {
                    return configurationFactory.NamespaceManager.GetTopic(topicName);
                });

                createNew = (topic == null);
            }
            catch (MessagingEntityNotFoundException)
            {
                // Looks like the topic does not exist. We should create a new one.
                createNew = true;
            }

            // If a topic with the specified name doesn't exist, it will be auto-created.
            if (createNew)
            {
                try
                {
                    var newTopic = new TopicDescription(topicName);
                    topic = retryPolicy.ExecuteAction<TopicDescription>(() =>
                    {
                        return configurationFactory.NamespaceManager.CreateTopic(newTopic);
                    });
                }
                catch (MessagingEntityAlreadyExistsException)
                {
                    // A topic under the same name was already created by someone else, perhaps by another instance. Let's just use it.
                    topic = retryPolicy.ExecuteAction<TopicDescription>(() =>
                    {
                        return configurationFactory.NamespaceManager.GetTopic(topicName);
                    });
                }
            }

        }

        public void Dispose()
        {
            Dispose(true);
            configurationFactory.MessageFactory.Close();
        }

        public abstract void Dispose(bool disposing);
    }
}
