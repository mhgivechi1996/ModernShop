using Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(CreateCategoryRequestValidator).Assembly;
        return services
            .AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>()
            .AddMediatR(assembly)
            .AddTransient<IHttpContextAccessor, HttpContextAccessor>();

    }
}