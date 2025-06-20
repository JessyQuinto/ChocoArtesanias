using AutoMapper;
using ChocoArtesanias.Application.Mappings;
using ChocoArtesanias.Application.Services;
using ChocoArtesanias.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ChocoArtesanias.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        // Application Services
        services.AddScoped<AuthService>();
        services.AddScoped<UserService>();
        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<ProducerService>();
        services.AddScoped<CartService>();
        services.AddScoped<OrderService>();

        return services;
    }
}
