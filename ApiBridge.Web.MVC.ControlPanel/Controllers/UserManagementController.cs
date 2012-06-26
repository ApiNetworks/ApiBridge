using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiBridge.Web.Interfaces;
using ApiBridge.Contracts;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class UserManagementController : Controller
    {
        private IUserManagementService userService;
        private ILogManagementService logService;

        public UserManagementController(IUserManagementService userService, ILogManagementService logService)
        {
            this.userService = userService;
            this.logService = logService;
        }


        //
        // GET: /UserManager/

        public ActionResult Index()
        {
            IEnumerable<User> users = userService.GetAllUsers();
            
            return View(users);
        }

        public ActionResult Log()
        {
            IEnumerable<User> users = logService.GetAllEvents();

            return View(users);
        }

    }
}
