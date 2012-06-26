using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Web.MVC.ControlPanel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiBridge.Web.MVC.ControlPanel.Tests.MockClasses
{
    public class MockFormsAuthenticationService : IFormsAuthenticationService
    {
        public bool SignIn_WasCalled;
        public bool SignOut_WasCalled;

        public void SignIn(string userName, bool createPersistentCookie)
        {
            // verify that the arguments are what we expected
            Assert.AreEqual("someUser", userName);
            Assert.IsFalse(createPersistentCookie);

            SignIn_WasCalled = true;
        }

        public void SignOut()
        {
            SignOut_WasCalled = true;
        }
    }
}
