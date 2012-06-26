using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Web.Interfaces;

namespace ApiBridge.Web.MVC.ControlPanel.Tests.MockClasses
{
    public class MockHelloWorldService : IHelloWorldService
    {
        public string SayHello(string input)
        {
            return String.Format("I'm a mock service. Your input was: {0}", input);
        }
    }
}
