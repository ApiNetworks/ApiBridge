﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Commands
{
    public class FetchCJAdvertisersCommand
    {
        public string GetCommandStr()
        {
            return "FetchCJAdvertisersEvent";
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

        public string WebServiceToken { get; set; }
        public string PublisherId { get; set; }
    }
}
