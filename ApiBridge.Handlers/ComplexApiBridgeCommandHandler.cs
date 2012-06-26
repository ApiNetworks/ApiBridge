using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ApiBridge.Handlers.Interfaces;
using ApiBridge.Commands;

namespace ApiBridge.Handlers
{
    public class ComplexApiBridgeCommandHandler : ICommandHandler<ComplexApiBridgeCommand>
    {
        public void Handle(ICommandReceiver<ComplexApiBridgeCommand> command)
        {
            if (command != null)
            {
                string myPayload = command.Body.Payload as string;
                Dictionary<string, string> meta = command.Body.Metadata;
                Console.WriteLine(myPayload);
            }
        }
    }
}
