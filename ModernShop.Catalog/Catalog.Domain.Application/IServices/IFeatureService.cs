using Catalog.Application.Contract.Common;
using Catalog.Application.Contract.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.IServices
{
    public interface IFeatureService
    {
        Task<CatalogActionResult<FeatureDto>> GetById(Guid id, Guid userId);
        Task<CatalogActionResult<FeatureDto>> GetList(GridQueryDto model = null);
        Task<CatalogActionResult<FeatureDto>> Add(FeatureDto feature);
        Task<CatalogActionResult<FeatureDto>> Update(FeatureDto feature);
        Task<CatalogActionResult<FeatureDto>> Delete(Guid featureId);

    }
}
