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
    public class UserManagementService : IUserManagementService
    {
        private readonly IDocumentSession session;

        public UserManagementService(IDocumentSession session)
        {
            this.session = session;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return session.Query<User>().OrderByDescending(u => u.DateCreated);
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
