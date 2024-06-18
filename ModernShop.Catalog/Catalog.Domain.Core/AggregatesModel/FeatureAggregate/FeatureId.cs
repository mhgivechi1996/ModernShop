using Catalog.Domain.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Core.AggregatesModel.FeatureAggregate
{
    public sealed class FeatureId: StronglyTypedId<FeatureId>
    {
        public FeatureId(Guid value): base(value)
        {

        }
    }
}
