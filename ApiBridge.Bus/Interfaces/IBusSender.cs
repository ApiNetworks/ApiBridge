using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Bus.Interfaces
{
    interface IBusSender
    {
        void Close();
        void Dispose(bool disposing);
        void Send<T>(T obj);
        void Send<T>(T obj, IDictionary<string, object> metadata = null);
        void Send<T>(T obj, IServiceBusSerializer serializer = null, IDictionary<string, object> metadata = null);
        void SendAsync<T>(T obj, object state, Action<ICommandSent<T>> resultCallBack);
        void SendAsync<T>(T obj, object state, Action<ICommandSent<T>> resultCallBack, IDictionary<string, object> metadata = null);
        void SendAsync<T>(T obj, object state, Action<ICommandSent<T>> resultCallBack, IServiceBusSerializer serializer = null, IDictionary<string, object> metadata = null);
    }
}
