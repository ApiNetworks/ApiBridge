using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using ApiBridge.Web.Interfaces;
using ApiBridge.Web.MVC.ControlPanel.Models;
using ApiBridge.Web.MVC.ControlPanel.Services;
using ApiBridge.Web.Services;

using Autofac;
using Autofac.Integration.Mvc;

using Raven.Client;
using Raven.Client.Embedded;
using Microsoft.Practices.ServiceLocation;
using ApiBridge.Core.Services;
using Raven.Client.Document;
using System.Web;
using ApiBridge.Web.MVC.ControlPanel.Controllers;
using ApiBridge.Commands;
using ApiBridge.Handlers;
using ApiBridge.Bus.Core;
using System.Configuration;
using ApiBridge.Bus;
using System.Threading;
using ApiBridge.Bus.Interfaces;


namespace ApiBridge.Web.MVC.ControlPanel
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            BeginRequest += (sender, args) =>
            {
                HttpContext.Current.Items["CurrentRequestRavenSession"] = BaseController.DocumentStore.OpenSession();
            };
            EndRequest += (sender, args) =>
            {
                using (var session = (IDocumentSession)HttpContext.Current.Items["CurrentRequestRavenSession"])
                {
                    if (session == null)
                        return;

                    if (Server.GetLastError() != null)
                        return;

                    session.SaveChanges();
                }
            };
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            InitializeDocumentStore();
            BaseController.DocumentStore = DocumentStore;

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(ApiBridge.Web.MVC.ControlPanel.MvcApplication).Assembly);
            
            builder.Register<MembershipProvider>(c => Membership.Provider);

            // RavenDB DataStore injection
            builder.RegisterType<DocumentStore>().As<IDocumentStore>();
            builder.RegisterInstance(GetDocumentStore()).SingleInstance();

            builder.RegisterType<DocumentSession>().As<IDocumentSession>();
            builder.Register<IDocumentSession>(c => MvcApplication.CurrentSession);
            
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            builder.RegisterType<AccountMembershipService>().As<IMembershipService>();

            
            // setup Service maps
            builder.RegisterType<HelloWorldService>().As<IHelloWorldService>();
            builder.RegisterType<UserManagementService>().As<IUserManagementService>();
            builder.RegisterType<LogManagementService>().As<ILogManagementService>();
            builder.RegisterType<CommentManagementService>().As<ICommentManagementService>();

            
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
            // Wire up service locator for membership provider
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

            // init service bus
            InitServiceBus(container);

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BaseController.ServiceBus = ServiceBus;
        }

        protected void Application_End()
        {
         
        }

        public static IBus Bus
        {
            get;
            set;
        }

        public static void InitServiceBus(IContainer container)
        {
            // ServiceBus init
            BusConfiguration.WithSettings()
            .UseAutofacContainer(container)
            .ServiceBusApplicationId("AppName")
            .ServiceBusIssuerKey("xxx")
            .ServiceBusIssuerName("xxx")
            .ServiceBusNamespace("xxx")
            .RegisterAssembly(typeof(SimpleApiBridgeCommandHandler).Assembly)
            .InboundTopicName("xx")
            .OutboundTopicName("xxx")
            .Configure();

            Bus = ServiceBus;
        }

        private IDocumentStore GetDocumentStore()
        {
            return DocumentStore;
        }

        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession)HttpContext.Current.Items["CurrentRequestRavenSession"]; }
        }

        public static IDocumentStore DocumentStore { get; private set; }

        public static IBus ServiceBus 
        {
            get
            {
                return BusConfiguration.Instance.Bus;
            }
        }

        private static void InitializeDocumentStore()
        {
            if (DocumentStore != null) return; // prevent misuse

            DocumentStore = new EmbeddableDocumentStore
            {
                DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Database")
            }.Initialize();

            DocumentStore.Conventions.IdentityPartsSeparator = "-";
        }
    }
}