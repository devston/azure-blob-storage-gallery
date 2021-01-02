using Autofac;
using Autofac.Integration.Mvc;
using AzureBlobService.Common.DependencyResolution;
using DependencyResolution;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AzureBlobService
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Set up DI.
            var builder = IoCBootstrapper.SetUpBuilder();

            // Register the MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register any model binders.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // Register presentation layer modules.
            builder.RegisterModule<PresentationModule>();

            // Set the dependency resolver to be autofac.
            var container = builder.Build();
            IoCBootstrapper.SetIoCContainer(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Dispose of the IoC container.
            IoCBootstrapper.DisposeContainer();
        }
    }
}
