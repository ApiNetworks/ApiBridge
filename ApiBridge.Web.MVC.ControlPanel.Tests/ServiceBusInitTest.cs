using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiBridge.Web.MVC.ControlPanel.Controllers;
using ApiBridge.Web.MVC.ControlPanel.Tests.MockClasses;
using ApiBridge.Web.MVC.ControlPanel.Models;
using System.Web.Mvc;
using ApiBridge.Bus.Core;


namespace ApiBridge.Web.MVC.ControlPanel.Tests
{
    [TestClass]
    public class ServiceBusInitTest
    {
        [TestMethod]
        public void TestServiceBusInitFromGlobalAsax()
        {
            //MvcApplication.InitServiceBus();
            Assert.AreNotEqual(null, MvcApplication.ServiceBus);
        }

        [TestMethod]
        public void TestServiceBusPublish()
        {
            // Arrange
            AccountController controller = AccountControllerTest.GetAccountController();
            BusConfiguration.WithSettings()
            .UseAutofacContainer()
            .ServiceBusApplicationId("AppName")
            .ServiceBusIssuerKey("2vunIjmTPh29/dwWGgjgP6XJmAgG9sGGbyhRPSqk5IQ=")
            .ServiceBusIssuerName("owner")
            .ServiceBusNamespace("apibridge")
            .TopicName("chat")
            .Configure();
            AccountController.ServiceBus = BusConfiguration.Instance.Bus;

            RegisterModel model = new RegisterModel()
            {
                UserName = "someUser",
                Email = "goodEmail",
                Password = "goodPassword",
                ConfirmPassword = "goodPassword"
            };

            // Act
            ActionResult result = controller.Register(model);
        }
    }
}
