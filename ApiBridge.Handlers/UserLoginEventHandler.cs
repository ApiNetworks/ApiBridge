using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Handlers.Interfaces;
using ApiBridge.Commands;
using ApiBridge.Contracts;

namespace ApiBridge.Handlers
{
    public class UserLoginEventHandler : ICommandHandler<UserLoginEvent>
    {
        public void Handle(ICommandReceiver<UserLoginEvent> command)
        {
            if (command != null)
            {
                string loginUserName = command.Body.User.Username;
                DateTime loginTime = command.Body.CreationDate;
                Console.WriteLine("User login: {0} at {1}", loginUserName, loginTime);
            }
        }
    }
}
