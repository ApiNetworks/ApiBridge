using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ApiBridge.Web.Interfaces;
using ApiBridge.Contracts;
using Raven.Client;
using Raven.Client.Linq;

namespace ApiBridge.Web.Services
{
    public class LogManagementService : ILogManagementService
    {
        private readonly IDocumentSession session;

        public LogManagementService(IDocumentSession session)
        {
            this.session = session;
        }

        public IEnumerable<User> GetAllEvents()
        {
            //return session.Query<CommonLoginUserEvent>().OrderByDescending(x => x.User.DateLastLogin).Select(x => x.User);
            return null;
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
