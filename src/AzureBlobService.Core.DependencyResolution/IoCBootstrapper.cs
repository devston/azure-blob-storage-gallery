using Autofac;
using AzureBlobService.Core.DependencyResolution.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobService.Core.DependencyResolution
{
    /// <summary>
    /// The IoC container bootstrapper.
    /// </summary>
    public static class IoCBootstrapper
    {
        /// <summary>
        /// Configure IoC Container using Autofac: Register DI.
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreServiceModule());
            builder.RegisterModule(new PresentationServiceModule());
        }
    }
}
