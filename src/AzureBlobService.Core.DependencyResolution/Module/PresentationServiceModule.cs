using Autofac;
using AzureBlobService.Presentation.Service.Storage;

namespace AzureBlobService.Core.DependencyResolution.Module
{
    /// <summary>
    /// The autofac module containing all of the presentation service registrations.
    /// </summary>
    public class  PresentationServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlobStoragePresentationService>().As<IBlobStoragePresentationService>()
                .InstancePerLifetimeScope();
        }
    }
}
