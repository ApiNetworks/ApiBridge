using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using ApiBridge.Bus.Core;
using ApiBridge.Bus.Interfaces;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public abstract class BaseController : Controller
    {
        public static IDocumentStore DocumentStore { get; set; }
        public static IBus ServiceBus { get; set; }

        public IDocumentSession RavenSession { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = (IDocumentSession)HttpContext.Items["CurrentRequestRavenSession"];
        }
    }
}
