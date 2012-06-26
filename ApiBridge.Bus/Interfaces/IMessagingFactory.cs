using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using ApiBridge.Bus.Interfaces;

namespace ApiBridge.Bus.Interfaces
{
    interface IMessagingFactory {
        SubscriptionClient CreateSubscriptionClient(string topicPath, string name, ReceiveMode receiveMode);
        TopicClient CreateTopicClient(string path);
        void Close();
        void Initialize(Uri serviceUri, TokenProvider tokenProvider);
    }
}
