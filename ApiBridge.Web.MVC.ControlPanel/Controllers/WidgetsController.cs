using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiBridge.Web.MVC.ControlPanel.Models;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class WidgetsController : BaseController
    {
        //
        // GET: /Widgets/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdRenderer()
        {
            AdRendererModel model = new AdRendererModel();

            return View(model);
        }

        public ActionResult LeadCapture()
        {
            LeadCaptureModel model = new LeadCaptureModel();

            return View(model);
        }

        public ActionResult PageImpression()
        {
            PageImpressionModel model = new PageImpressionModel();

            return View(model);
        }
    }
}
