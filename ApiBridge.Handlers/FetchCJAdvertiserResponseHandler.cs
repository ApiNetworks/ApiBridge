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
    public class FetchCJAdvertiserResponseHandler : ICommandHandler<FetchCJAdvertiserResponse>
    {
        public void Handle(ICommandReceiver<FetchCJAdvertiserResponse> ev)
        {
            if (BusConfiguration.Instance.Container.IsRegistered(typeof(IDocumentStore)))
            {
                IDocumentStore documentStore = BusConfiguration.Instance.Container.Resolve<IDocumentStore>();
                if (documentStore != null)
                {
                    using (IDocumentSession session = documentStore.OpenSession())
                    {
                        session.Store(ev.Body.Advertiser);
                        session.SaveChanges();
                    }
                }
            }
            else
            {
                Console.WriteLine(ev.Body.Advertiser.AdvertiserName);
            }
        }
    }
}
