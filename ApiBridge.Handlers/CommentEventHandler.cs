using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Handlers.Interfaces;
using ApiBridge.Commands;
using Raven.Client;
using ApiBridge.Bus.Core;

namespace ApiBridge.Handlers
{
    public class CommentEventHandler : ICommandHandler<CommentEvent>
    {
        public void Handle(ICommandReceiver<CommentEvent> command)
        {
            if (BusConfiguration.Instance.Container.IsRegistered(typeof(IDocumentStore)))
            {
                IDocumentStore documentStore = BusConfiguration.Instance.Container.Resolve<IDocumentStore>();
                if (documentStore != null)
                {
                    using (IDocumentSession session = documentStore.OpenSession())
                    {
                        session.Store(command.Body.Comment);
                        session.SaveChanges();
                    }
                }
            }
            else
            {
                Console.WriteLine(command.Body.Comment.Text);
            }
        }
    }
}
