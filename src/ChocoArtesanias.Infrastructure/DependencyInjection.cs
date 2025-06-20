using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Infrastructure.Data;
using ChocoArtesanias.Infrastructure.Repositories;
using ChocoArtesanias.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChocoArtesanias.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProducerRepository, ProducerRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
