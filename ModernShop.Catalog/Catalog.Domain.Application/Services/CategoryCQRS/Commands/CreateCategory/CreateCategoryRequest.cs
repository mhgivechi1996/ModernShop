using Catalog.Application.Common.FileStorage;
using Catalog.Domain.Core;
using Catalog.Domain.Core.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory
{
    public class CreateCategoryRequest : IRequest<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public List<Guid>? Features { get; set; } = null;
        public Guid? UserId { get; set; }
    }

    public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, Guid>
    {
        private readonly Domain.Core.SeedWork.Repository.IRepository<Category> _categoryRepository;
        private readonly IFileStorageService _fileService;
        private IValidator<CreateCategoryRequest> _validator;

        public CreateCategoryRequestHandler(Domain.Core.SeedWork.Repository.IRepository<Category> categoryRepository,
            IFileStorageService fileService,
            IValidator<CreateCategoryRequest> validator) =>
            (_categoryRepository, _fileService, _validator) = (categoryRepository, fileService,validator);

        public async Task<Guid> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
           var validationResult = await _validator.ValidateAsync(request);
            if(!validationResult.IsValid)
            {
                throw new Exception(String.Join(",",validationResult.Errors.Select(q => q.ErrorMessage).ToArray()));
            }

            var thumbnailSaveResult = await _fileService.UploadAsync<Category>(request.Thumbnail, FileType.Image, "Category", request.UserId.Value, cancellationToken);


            var category = Category.CreateNew(request.Name, request.IsActive, request.Description, request.Features,
                thumbnailSaveResult?.FilePath, thumbnailSaveResult?.FileName, thumbnailSaveResult?.FileExtension, thumbnailSaveResult?.FileSize);

            await _categoryRepository.AddAsync(category, cancellationToken);

            return category.Id.Value;
        }
    }
}
