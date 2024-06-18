using Catalog.Domain.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Core.AggregatesModel.FeatureAggregate
{
    public interface IFeatureRepository : IRepository<Feature>
    {
        Task<Feature> Add(Feature feature);
        Feature Update(Feature feature);
        Task<Feature> FindByIdAsync(Guid featureId);
    }

}
