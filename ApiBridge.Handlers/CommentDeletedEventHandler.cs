using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Handlers.Interfaces;
using ApiBridge.Commands;

namespace ApiBridge.Handlers
{
    public class CommentDeletedEventHandler : ICommandHandler<CommentDeletedEvent>
    {
        public void Handle(ICommandReceiver<CommentDeletedEvent> command)
        {
            if (command != null)
            {
                Console.WriteLine(command.Body.Message);
            }
        }
    }
}
