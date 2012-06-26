using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Commands
{
    public class ComplexApiBridgeCommand
    {
        public int RetryCount { get; set; }

        public string Payload
        {
            get;
            set;
        }

        public Dictionary<string, string> Metadata
        {
            get;
            set;
        }
    }
}
