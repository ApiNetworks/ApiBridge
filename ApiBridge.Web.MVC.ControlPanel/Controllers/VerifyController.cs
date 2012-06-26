using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiBridge.Web.MVC.ControlPanel.Models;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class VerifyController : BaseController
    {
        //
        // GET: /Verify/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(VerifyModel model)
        {

            return RedirectToAction("Index", "Home");
        }
    }
}
