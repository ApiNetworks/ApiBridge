using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Handlers;
using ApiBridge.Commands;
using ApiBridge.Bus.Core;
using ApiBridge.Contracts;
using System.Threading;

namespace ApiBridge.ConsoleService
{
    class Program
    {
        public static void Initialize()
        {
            BusConfiguration.WithSettings()
                .UseAutofacContainer()
                .ServiceBusApplicationId("xx")
                .ServiceBusNamespace("xx")
                .ServiceBusIssuerName("xx")
                .ServiceBusIssuerKey("xx")
                .InboundTopicName("xx")
                .OutboundTopicName("xx")
                .RegisterAssembly(typeof(UserLoginEventHandler).Assembly)
                .Configure();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to ApiBridge Service Console!");

            Initialize();

            // wait to initialize
            Thread.Sleep(5000);
            Console.WriteLine("Ready...");

            while (true)
            {
                Comment myComment = new Comment();
                myComment.Text = "My Comment: " + Console.ReadLine();

                CommentEvent commentEvent = new CommentEvent();
                commentEvent.Comment = myComment;

                BusConfiguration.Instance.Bus.PublishAsync<CommentEvent>(commentEvent, (result) =>
                {
                    Console.WriteLine("Sent:" + result.IsSuccess);
                });
            }
        }
    }
}
