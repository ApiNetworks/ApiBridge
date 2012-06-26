using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Contracts
{
    public class User
    {
        public string Id { get; set; }
        public string ApplicationName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateLastLogin { get; set; }
        
        public List<string> Roles { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public User()
        {
            Id = "authorization/users/"; // db assigns id
            Roles = new List<string>();
            Properties = new Dictionary<string, string>();
        }
    }
}
