using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Contracts;

namespace ApiBridge.Commands
{
    public class UserLoginEvent
    {
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
