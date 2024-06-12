using Ardalis.Specification;
using Catalog.Domain.Core;
using Catalog.Domain.Core.AggregatesModel.ProductAggregate;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalog.Application.Services.CategoryCQRS;

public class CategoryByNameSpec : Specification<Category>, ISingleResultSpecification
{
    public CategoryByNameSpec(string name) =>
        Query.Where(p => p.CategoryName == name);
}
