using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using E_LearningPlatform.Application.Mapping;

namespace Companion.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });
        services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);
        return services;
    }
}

