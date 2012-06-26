using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Web.MVC.ControlPanel.Services;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiBridge.Web.MVC.ControlPanel.Tests.MockClasses
{
    public class MockMembershipService : IMembershipService
    {
        public int MinPasswordLength
        {
            get { return 10; }
        }

        public bool ValidateUser(string userName, string password)
        {
            return (userName == "someUser" && password == "goodPassword");
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (userName == "duplicateUser")
            {
                return MembershipCreateStatus.DuplicateUserName;
            }

            // verify that values are what we expected
            Assert.AreEqual("goodPassword", password);
            Assert.AreEqual("goodEmail", email);

            return MembershipCreateStatus.Success;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return (userName == "someUser" && oldPassword == "goodOldPassword" && newPassword == "goodNewPassword");
        }
    }
}
