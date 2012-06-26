using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Template()
        {
            return View("_template");
        }
    }
}
