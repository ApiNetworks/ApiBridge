using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ApiBridge.Bus.Interfaces
{
    interface IServiceBusConfigurationFactory {

        IMessagingFactory MessageFactory {
            get;
        }

        INamespaceManager NamespaceManager {
            get;
        }

    }
}
