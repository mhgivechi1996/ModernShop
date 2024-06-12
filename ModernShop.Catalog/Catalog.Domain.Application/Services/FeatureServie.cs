using Catalog.Application.Contract.Common;
using Catalog.Application.Contract.Dtos;
using Catalog.Application.IServices;
using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Services
{
    public class FeatureServie : IFeatureService
    {
        private readonly IFeatureRepository featureRepository;
        public FeatureServie(IFeatureRepository featureRepository)
        {
            this.featureRepository = featureRepository;
        }
       
        public async Task<CatalogActionResult<FeatureDto>> Add(FeatureDto model)
        {
            var result = new CatalogActionResult<FeatureDto>();

            var feature = Feature.CreateNew(model.Title, model.Description, model.SortOrder);
            var featureAfterInsert = await featureRepository.Add(feature);
            await featureRepository.UnitOfWork.SaveEntitiesAsync();

            model.Id = featureAfterInsert.Id.Value;

            result.IsSuccess = true;
            result.Data = model;
            return result;
        }

        public async Task<CatalogActionResult<FeatureDto>> Delete(Guid featureId)
        {
            throw new NotImplementedException();
        }

        public async Task<CatalogActionResult<FeatureDto>> GetById(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<CatalogActionResult<FeatureDto>> GetList(GridQueryDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<CatalogActionResult<FeatureDto>> Update(FeatureDto feature)
        {
            throw new NotImplementedException();
        }
    }
}
