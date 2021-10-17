using Autofac;
using AzureBlobService.Core.Service.Storage;

namespace AzureBlobService.Core.DependencyResolution.Module
{
    /// <summary>
    /// The autofac module containing all of the core service registrations.
    /// </summary>
    public class CoreServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlobStorageService>().As<IBlobStorageService>()
                .InstancePerLifetimeScope();
        }
    }
}
