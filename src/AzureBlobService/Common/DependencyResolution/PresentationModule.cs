using Autofac;
using AzureBlobService.Common.Services.Storage;

namespace AzureBlobService.Common.DependencyResolution
{
    public class PresentationModule : Module
    {
        /// <summary>
        /// Load the module.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlobStorageService>()
                .As<IBlobStorageService>();
        }
    }
}