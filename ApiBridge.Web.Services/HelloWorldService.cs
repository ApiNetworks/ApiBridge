using System;

using ApiBridge.Web.Interfaces;

namespace ApiBridge.Web.Services
{
    public class HelloWorldService : IHelloWorldService
    {
        public string SayHello(string input)
        {
            return String.Format("Hello world! Your input was: {0}.", input);
        }
    }
}
