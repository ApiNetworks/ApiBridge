using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ApiBridge.Web.MVC.ControlPanel.Models;
using ApiBridge.Web.Interfaces;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class TestController : BaseController
    {
        // sample service
        private IHelloWorldService _helloWorldService;

        public TestController(IHelloWorldService hellowWorldService)
        {
            _helloWorldService = hellowWorldService;
        }

        public ContentResult HelloWorld()
        {
            ContentResult contentResult = new ContentResult();
            contentResult.Content = _helloWorldService.SayHello(DateTime.Now.ToString());

            return contentResult;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
