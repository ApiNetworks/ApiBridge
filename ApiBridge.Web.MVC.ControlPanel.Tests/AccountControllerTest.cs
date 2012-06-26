using ApiBridge.Web.MVC.ControlPanel.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using ApiBridge.Web.MVC.ControlPanel.Services;
using ApiBridge.Web.MVC.ControlPanel.Models;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Security.Principal;
using System.Web.Security;
using ApiBridge.Web.MVC.ControlPanel.Tests.MockClasses;

namespace ApiBridge.Web.MVC.ControlPanel.Tests
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void ChangePassword_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePassword();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(10, ((ViewResult)result).ViewData["PasswordLength"]);
        }

        [TestMethod]
        public void ChangePassword_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel()
            {
                OldPassword = "goodOldPassword",
                NewPassword = "goodNewPassword",
                ConfirmPassword = "goodNewPassword"
            };

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("ChangePasswordSuccess", redirectResult.RouteValues["action"]);
        }

        [TestMethod]
        public void ChangePassword_Post_ReturnsViewIfChangePasswordFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel()
            {
                OldPassword = "goodOldPassword",
                NewPassword = "badNewPassword",
                ConfirmPassword = "badNewPassword"
            };

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual("The current password is incorrect or the new password is invalid.", controller.ModelState[""].Errors[0].ErrorMessage);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }

        [TestMethod]
        public void ChangePassword_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel()
            {
                OldPassword = "goodOldPassword",
                NewPassword = "goodNewPassword",
                ConfirmPassword = "goodNewPassword"
            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.ChangePassword(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }

        [TestMethod]
        public void ChangePasswordSuccess_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePasswordSuccess();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void LogOff_LogsOutAndRedirects()
        {
            // Arrange
            var mockFormsAuthenticationService = new MockFormsAuthenticationService();
            var mockMembershipService = new MockMembershipService();
            AccountController controller = GetAccountController(mockFormsAuthenticationService,
                                                                    mockMembershipService);

            // Act
            ActionResult result = controller.LogOff();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(mockFormsAuthenticationService.SignOut_WasCalled);
        }

        [TestMethod]
        public void LogOn_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.LogOn();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithoutReturnUrl()
        {
            // Arrange
            var mockFormsAuthenticationService = new MockFormsAuthenticationService();
            var mockMembershipService = new MockMembershipService();
            AccountController controller = GetAccountController(mockFormsAuthenticationService,
                                                                    mockMembershipService);

            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "goodPassword",
                RememberMe = false
            };

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(mockFormsAuthenticationService.SignIn_WasCalled);
        }

        [TestMethod]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithLocalReturnUrl()
        {
            // Arrange
            var mockFormsAuthenticationService = new MockFormsAuthenticationService();
            var mockMembershipService = new MockMembershipService();
            AccountController controller = GetAccountController(mockFormsAuthenticationService,
                                                                    mockMembershipService);
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "goodPassword",
                RememberMe = false
            };

            // Act
            ActionResult result = controller.LogOn(model, "/someUrl");

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            RedirectResult redirectResult = (RedirectResult)result;
            Assert.AreEqual("/someUrl", redirectResult.Url);
            Assert.IsTrue(mockFormsAuthenticationService.SignIn_WasCalled);
        }

        [TestMethod]
        public void LogOn_Post_ReturnsRedirectToHomeOnSuccess_WithExternalReturnUrl()
        {
            // Arrange
            var mockFormsAuthenticationService = new MockFormsAuthenticationService();
            var mockMembershipService = new MockMembershipService();
            AccountController controller = GetAccountController(mockFormsAuthenticationService,
                                                                    mockMembershipService);
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "goodPassword",
                RememberMe = false
            };

            // Act
            ActionResult result = controller.LogOn(model, "http://malicious.example.net");

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(mockFormsAuthenticationService.SignIn_WasCalled);
        }

        [TestMethod]
        public void LogOn_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "goodPassword",
                RememberMe = false
            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
        }

        [TestMethod]
        public void LogOn_Post_ReturnsViewIfValidateUserFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel()
            {
                UserName = "someUser",
                Password = "badPassword",
                RememberMe = false
            };

            // Act
            ActionResult result = controller.LogOn(model, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual("The user name or password provided is incorrect.", controller.ModelState[""].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void Register_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.Register();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(10, ((ViewResult)result).ViewData["PasswordLength"]);
        }

        [TestMethod]
        public void Register_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel()
            {
                UserName = "someUser",
                Email = "goodEmail",
                Password = "goodPassword",
                ConfirmPassword = "goodPassword"
            };

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Register_Post_ReturnsViewIfRegistrationFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel()
            {
                UserName = "duplicateUser",
                Email = "goodEmail",
                Password = "goodPassword",
                ConfirmPassword = "goodPassword"
            };

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual("Username already exists. Please enter a different user name.", controller.ModelState[""].Errors[0].ErrorMessage);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }

        [TestMethod]
        public void Register_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            RegisterModel model = new RegisterModel()
            {
                UserName = "someUser",
                Email = "goodEmail",
                Password = "goodPassword",
                ConfirmPassword = "goodPassword"
            };
            controller.ModelState.AddModelError("", "Dummy error message.");

            // Act
            ActionResult result = controller.Register(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual(10, viewResult.ViewData["PasswordLength"]);
        }

        public static AccountController GetAccountController()
        {
            var mockFormsAuthenticationService = new MockFormsAuthenticationService();
            var mockMembershipService = new MockMembershipService();
            return GetAccountController(mockFormsAuthenticationService, mockMembershipService);
        }

        public static AccountController GetAccountController(MockFormsAuthenticationService mockFormsAuthenticationService, MockMembershipService mockMembershipService)
        {
            RequestContext requestContext = new RequestContext(new MockHttpContext(), new RouteData());
            AccountController controller = new AccountController(mockFormsAuthenticationService, mockMembershipService)
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
    }    
    
}
