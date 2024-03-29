﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using ApiBridge.Bus.Interfaces;
using Autofac;

namespace ApiBridge.Bus.Factories
{
    class ServiceBusConfigurationFactory : IServiceBusConfigurationFactory {

        IBusConfiguration configuration;
        IMessagingFactory messageFactory;
        INamespaceManager namespaceManager;
        string servicePath;
        Uri serviceUri;
        TokenProvider tokenProvider;

        public ServiceBusConfigurationFactory(IBusConfiguration configuration) {
            Guard.ArgumentNotNull(configuration, "configuration");

            this.configuration = configuration;
            servicePath = string.Empty;
            if (!string.IsNullOrWhiteSpace(configuration.ServicePath)) {
                servicePath = configuration.ServicePath;
            }

        }

        public IMessagingFactory MessageFactory {
            get {
                if (messageFactory == null) {
                    messageFactory = configuration.Container.Resolve<IMessagingFactory>();
                    messageFactory.Initialize(ServiceUri, TokenProvider);
                }
                return messageFactory;
            }
        }

        public INamespaceManager NamespaceManager {
            get {
                if (namespaceManager == null) {
                    namespaceManager = configuration.Container.Resolve<INamespaceManager>();
                    namespaceManager.Initialize(ServiceUri, TokenProvider);
                }
                return namespaceManager;
            }
        }

        TokenProvider TokenProvider {
            get {
                if (tokenProvider == null) {
                    tokenProvider = TokenProvider.CreateSharedSecretTokenProvider(configuration.ServiceBusIssuerName, configuration.ServiceBusIssuerKey);
                }
                return tokenProvider;
            }
        }

        Uri ServiceUri {
            get {
                if (serviceUri == null) {
                    serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", configuration.ServiceBusNamespace, servicePath);
                }
                return serviceUri;
            }
        }
    }
}
