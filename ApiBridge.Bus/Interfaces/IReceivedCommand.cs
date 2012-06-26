using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace ApiBridge.Bus.Interfaces
{
    public interface I2ReceivedCommand<T>
    {
        BrokeredMessage BrokeredMessage
        {
            get;
        }

        T Message
        {
            get;
        }

        IDictionary<string, object> Metadata
        {
            get;
        }
    }
}
