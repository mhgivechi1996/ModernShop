using Catalog.Application.Common.Validation;
using Catalog.Domain.Core;
using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using Catalog.Domain.Core.SeedWork.Repository;
using FluentValidation;

namespace Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory;

public class CreateCategoryRequestValidator : CustomValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator(IReadRepository<Category> categoryRepo, IReadRepository<Feature> featureRepo)
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (name, ct) => await categoryRepo.GetBySpecAsync(new CategoryByNameSpec(name)) is null)
                .WithMessage((_, name) => $"Category {name} already Exists.");


        RuleForEach(c => c.Features)
            .NotEmpty().MustAsync(async (id, ct) => await featureRepo.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => $"Feature {id} Not Found.");
    }
}