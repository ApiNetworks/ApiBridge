using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ApiBridge.Commands;
using ApiBridge.Handlers.Interfaces;

namespace ApiBridge.Handlers
{
    public class SimpleApiBridgeCommandHandler : ICommandHandler<ApiBridgeCommand>
    {
        public void Handle(ICommandReceiver<ApiBridgeCommand> command)
        {
            if (command != null)
            {
                string myPayload = command.Body.Payload as string;
                Console.WriteLine(myPayload);
            }
        }
    }
}
