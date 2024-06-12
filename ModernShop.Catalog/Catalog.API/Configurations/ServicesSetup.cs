using Catalog.Application.IServices;
using Catalog.Application.Services;
using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;

namespace Catalog.API.Configurations
{
    public static class ServicesSetup
    {
        public static void AddServicesSetup(this IServiceCollection services)
        {
            services.AddScoped<IFeatureService, FeatureServie>();
        }
    }
}
