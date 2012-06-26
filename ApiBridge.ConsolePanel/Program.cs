using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven;
using Raven.Client;
using Raven.Client.Embedded;

using System.IO;
using System.Configuration;
using Raven.Client.Document;
using ApiBridge.Web.Interfaces;
using ApiBridge.Web.Services;
using ApiBridge.Contracts;
using ApiBridge.Handlers;
using ApiBridge.Commands;
using ApiBridge.Bus.Core;
using System.Threading;
using ApiBridge.Commands;

namespace ApiBridge.ConsolePanel
{
    class Program
    {
        public static IDocumentStore DocumentStore { get; private set; }

        private static void InitializeDocumentStore()
        {
            if (DocumentStore != null) return; // prevent misuse
            DocumentStore = new EmbeddableDocumentStore
            {
                ConnectionStringName = "RavenDB"
            }.Initialize();

            DocumentStore.Conventions.IdentityPartsSeparator = "-";
        }

        public static void Initialize()
        {
            BusConfiguration.WithSettings()
                .UseAutofacContainer()
                .ServiceBusApplicationId("xxx")
                .ServiceBusNamespace("xxx")
                .ServiceBusIssuerName("xxx")
                .ServiceBusIssuerKey("xxx")
                .InboundTopicName("xxx")
                .OutboundTopicName("xxx")
                .RegisterAssembly(typeof(ComplexApiBridgeCommandHandler).Assembly)
                .Configure();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to ApiBridge Remote Console!");

            Initialize();
            Thread.Sleep(5000);

            Console.WriteLine("Ready...");
            
            while (true)
            {
                Console.Write("Enter Text: ");
                ApiBridgeCommand cmd = new ApiBridgeCommand();
                cmd.Payload = Console.ReadLine();
                BusConfiguration.Instance.Bus.PublishAsync<ApiBridgeCommand>(cmd, (result) =>
                {
                    Console.WriteLine("Sent:" + result.IsSuccess);
                });
            }
        }

        #region obsolete

        //static void Main(string[] args)
        //{
        //    InitializeDocumentStore();

        //    using (IDocumentSession session = DocumentStore.OpenSession())
        //    {
        //        IUserManagementService userService = new UserManagementService(session);
        //        IEnumerable<User> users = userService.GetAllUsers();

        //        foreach (User user in users)
        //        {
        //            Console.WriteLine(user.Username);
        //        }
        //        Console.ReadKey();
        //    }
        //}

        //private static ServiceBusEndpointInfo topicEndpoint;

        //static void Main(string[] args)
        //{
        //    var serviceBusSettings = ConfigurationManager.GetSection(ServiceBusConfigurationSettings.SectionName) as ServiceBusConfigurationSettings;
        //    topicEndpoint = serviceBusSettings.Endpoints.Get(serviceBusSettings.DefaultEndpoint);

        //    Console.WriteLine("Welcome to Server: " +  ConfigurationManager.AppSettings["RoleInstanceId"]);

        //    ApiBridgeCommandSettings settings = new ApiBridgeCommandSettings();
        //    settings.EnableCarbonCopy = false;
        //    settings.EventWaitTimeout = TimeSpan.FromSeconds(5);
        //    settings.UseCompetingConsumers = true;
        //    settings.EnableAsyncPublish = true;

        //    using (InterRoleCommunicationExtension serverListener = new InterRoleCommunicationExtension(topicEndpoint, settings))
        //    {
        //        ApiBridgeCommandHandler handler = new ApiBridgeCommandHandler(serverListener);
        //        serverListener.Subscribe(handler);

        //        //ApiBridgeCommandServiceHandler handler2 = new ApiBridgeCommandServiceHandler(serverListener);
        //        //serverListener.Subscribe(handler2);

        //        while (true)
        //        {
        //            Thread.Sleep(1000);
        //        }
        //    }
        //}

        #endregion
    }
}
