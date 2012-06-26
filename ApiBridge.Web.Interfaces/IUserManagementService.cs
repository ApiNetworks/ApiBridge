using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Contracts;

namespace ApiBridge.Web.Interfaces
{
    public interface IUserManagementService : IDisposable
    {
        IEnumerable<User> GetAllUsers();
    }
}
