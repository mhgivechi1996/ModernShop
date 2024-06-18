using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using Catalog.Infrastructure.Database.Context;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Configurations
{
    public static class RepositoriesSetup
    {
        public static void AddRepositorySetup(this IServiceCollection services)
        {
          services.AddScoped<IFeatureRepository, FeatureRepositoy>();
        }
    }
}
