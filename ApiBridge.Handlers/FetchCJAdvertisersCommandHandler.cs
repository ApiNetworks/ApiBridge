using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Handlers.Interfaces;
using ApiBridge.Commands;

namespace ApiBridge.Handlers
{
    public class FetchCJAdvertisersCommandHandler : ICommandHandler<FetchCJAdvertisersCommand>
    {
        public void Handle(ICommandReceiver<FetchCJAdvertisersCommand> command)
        {
            if (String.IsNullOrEmpty(command.Body.PublisherId))
                return;
            if (String.IsNullOrEmpty(command.Body.WebServiceToken))
                return;
        }
    }
}
