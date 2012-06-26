using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Handlers.Interfaces
{
    public interface IMessageHandler<T>
    {
        /// <summary>
        /// Process a Message with the given signature.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        /// <remarks>
        /// Every message received by the bus with this message type will call this method.
        /// </remarks>
        void Handle(ICommandReceiver<T> command);

    }

    public interface ICommandHandler<T> : IMessageHandler<T>
    {
        
    }

    public interface ICompetingCommandHandler<T> : IMessageHandler<T>
    {
        
    }
}
