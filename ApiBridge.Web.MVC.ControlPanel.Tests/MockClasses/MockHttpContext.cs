using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;

namespace ApiBridge.Web.MVC.ControlPanel.Tests.MockClasses
{
    public class MockHttpContext : HttpContextBase
    {
        private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);
        private readonly HttpRequestBase _request = new MockHttpRequest();

        public override IPrincipal User
        {
            get
            {
                return _user;
            }
            set
            {
                base.User = value;
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _request;
            }
        }
    }
}
