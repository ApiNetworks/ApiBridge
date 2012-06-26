using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Bus.Interfaces;

namespace ApiBridge.Bus.Core
{
    /// <summary>
    /// Result of a call to SendAsyc on the message bus
    /// </summary>
    public class CommandSent<T> : ICommandSent<T>
    {

        public bool IsSuccess
        {
            get;
            set;
        }

        public Exception ThrownException
        {
            get;
            set;
        }

        public TimeSpan TimeSpent
        {
            get;
            set;
        }

        public object State
        {
            get;
            set;
        }

    }
}
