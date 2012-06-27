using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Contracts;

namespace ApiBridge.Commands
{
    public class FetchCJAdvertiserResponse
    {
        public string GetCommandStr()
        {
            return "FetchCJAdvertiserResponse";
        }

        bool handled;
        public bool Handled
        {
            get
            {
                return handled;
            }
            set
            {
                handled = value;
            }
        }

        public Advertiser Advertiser { get; set; }
    }
}
