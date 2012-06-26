using System;
using System.Web.Mvc;
using System.Web.Security;

using ApiBridge.Contracts;
using ApiBridge.Web.MVC.ControlPanel.Models;
using ApiBridge.Web.MVC.ControlPanel.Services;
using ApiBridge.Web.MVC.ControlPanel.Validation;
using ApiBridge.Commands;
using System.Collections.Generic;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class AccountController : BaseController
    {

        private IFormsAuthenticationService _formsService;
        private IMembershipService _membershipService;

        public AccountController(IFormsAuthenticationService formsService,
                                    IMembershipService membershipService)
        {
            _formsService = formsService;
            _membershipService = membershipService;
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_membershipService.ValidateUser(model.UserName, model.Password))
                {
                    _formsService.SignIn(model.UserName, model.RememberMe);

                    for (int i = 0; i < 5; i++)
                    {
                        User u = new User();
                        u.Username = model.UserName + " " + i.ToString();
                        u.DateLastLogin = DateTime.UtcNow;
                        u.DateCreated = DateTime.UtcNow;
                        u.Roles = new List<string>();

                        UserLoginEvent loginEvent = new UserLoginEvent();
                        loginEvent.CreationDate = DateTime.Now;
                        loginEvent.User = u;

                        BaseController.ServiceBus.PublishAsync<UserLoginEvent>(loginEvent, (result) => { });
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            _formsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = _membershipService.CreateUser(model.UserName, model.Password, model.Email);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    _formsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;

            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (_membershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = _membershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

    }
}
