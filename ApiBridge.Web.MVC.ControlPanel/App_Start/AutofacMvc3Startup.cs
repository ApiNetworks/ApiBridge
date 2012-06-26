using System.Web.Mvc;
using System.Web.Security;

using ApiBridge.Web.Interfaces;
using ApiBridge.Web.MVC.ControlPanel.Services;
using ApiBridge.Web.Services;

using Autofac;
using Autofac.Integration.Mvc;
using Raven.Client.Document;
using Raven.Client;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(ApiBridge.Web.MVC.ControlPanel.App_Start.AutofacMvcStartup), "PreStart")]

namespace ApiBridge.Web.MVC.ControlPanel.App_Start
{
    public static class AutofacMvcStartup
    {
        public static void PreStart()
        {
            //ContainerBuilder builder = new ContainerBuilder();
            //builder.RegisterControllers(typeof(ApiBridge.Web.MVC.ControlPanel.MvcApplication).Assembly);
            
            //builder.Register<MembershipProvider>(c => Membership.Provider);

            //builder.Register<IDocumentStore>(c=> 
            
            //builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            //builder.RegisterType<AccountMembershipService>().As<IMembershipService>();
            
            //// setup Service maps
            //builder.RegisterType<HelloWorldService>().As<IHelloWorldService>();

            //IContainer container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}


