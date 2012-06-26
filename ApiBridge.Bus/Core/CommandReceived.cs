using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Bus.Interfaces;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Practices.TransientFaultHandling;
using ApiBridge.Handlers.Interfaces;

namespace ApiBridge.Bus.Core
{
    public class CommandReceived<T> : ICommandReceiver<T>
    {
        BrokeredMessage brokeredMessage;
        T body;
        IDictionary<string, object> metadata;

        public CommandReceived(BrokeredMessage brokeredMessage, T body, IDictionary<string, object> metadata)
        {
            Guard.ArgumentNotNull(brokeredMessage, "brokeredMessage");
            Guard.ArgumentNotNull(body, "message");
            Guard.ArgumentNotNull(metadata, "metadata");
            this.brokeredMessage = brokeredMessage;
            this.body = body;
            this.metadata = metadata;
        }

        public BrokeredMessage BrokeredMessage
        {
            get
            {
                return brokeredMessage;
            }
        }

        public IDictionary<string, object> Metadata
        {
            get
            {
                return metadata;
            }
        }

        public T Body
        {
            get 
            {
                return this.body;
            }
        }
    }
}
