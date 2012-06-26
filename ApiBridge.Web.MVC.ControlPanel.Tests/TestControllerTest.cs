using ApiBridge.Web.MVC.ControlPanel.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ApiBridge.Web.MVC.ControlPanel.Tests.MockClasses;
using ApiBridge.Web.MVC.ControlPanel.Models;
using ApiBridge.Web.Interfaces;
using System.Threading.Tasks;

using Autofac;
using Autofac.Integration.Mvc;

namespace ApiBridge.Web.MVC.ControlPanel.Tests
{
    /// <summary>
    ///This is a test class for TestControllerTest and is intended
    ///to contain all TestControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TestControllerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        private static TestController GetTestController()
        {
            RequestContext requestContext = new RequestContext(new MockHttpContext(), new RouteData());
            IHelloWorldService hellowWorldService = new MockHelloWorldService();
            TestController controller = new TestController(hellowWorldService)
            {
                Url = new UrlHelper(requestContext),
            };
            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = requestContext
            };
            return controller;
        }

        /// <summary>
        ///A test for HelloWorld
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        public void HelloWorldTest()
        {
            // resolve mock service
            var mockService = GetContainer().Resolve<IHelloWorldService>();

            // Use default constructor with resolved mock service
            TestController target = new TestController(mockService); 

            ContentResult actual;
            actual = target.HelloWorld();

            Assert.IsTrue(actual.Content.StartsWith("I'm a mock service."));
        }

        private IContainer GetContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // setup Service maps
            builder.RegisterType<MockHelloWorldService>().As<IHelloWorldService>();

            return builder.Build();
        }
    }
}
