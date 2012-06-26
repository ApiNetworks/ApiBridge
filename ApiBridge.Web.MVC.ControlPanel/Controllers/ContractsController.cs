using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class ContractsController : BaseController
    {
        //
        // GET: /Contract/

        public ActionResult Index()
        {
            return View();
        }
    }
}
