using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Bus.Interfaces
{
    public interface ICommandSent<T>
    {
        bool IsSuccess
        {
            get;
            set;
        }
        Exception ThrownException
        {
            get;
            set;
        }
        TimeSpan TimeSpent
        {
            get;
            set;
        }
        object State
        {
            get;
            set;
        }
    }
}
