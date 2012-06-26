using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Bus.Core;

namespace ApiBridge.Bus.Interfaces
{
    interface IBusReceiver
    {
        void CancelSubscription(ServiceBusEnpointData value);
        void CreateSubscription(ServiceBusEnpointData value);
        void Dispose(bool disposing);
    }
}
