using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Handlers.Interfaces
{
    public interface ICommandReceiver<T>
    {
        T Body
        {
            get;
        }

        IDictionary<string, object> Metadata
        {
            get;
        }
    }
}
