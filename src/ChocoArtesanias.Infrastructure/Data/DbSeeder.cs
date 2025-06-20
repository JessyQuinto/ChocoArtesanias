using ChocoArtesanias.Domain.Entities;
using ChocoArtesanias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChocoArtesanias.Infrastructure.Data;

public static class DbSeeder
{    public static async Task SeedAsync(AppDbContext context)
    {
        // Check if data already exists
        if (await context.Categories.AnyAsync()) return;        // Create predefined Guids for categories and producers
        var categoryJoyeriaId = Guid.NewGuid();
        var categoryTextilesId = Guid.NewGuid();
        var categoryCesteriaId = Guid.NewGuid();
        var categoryCeramicaId = Guid.NewGuid();
        var categoryTallasId = Guid.NewGuid();

        var producer1Id = Guid.NewGuid();
        var producer2Id = Guid.NewGuid();
        var producer3Id = Guid.NewGuid();

        // Seed Categories
        var categories = new List<Category>
        {
            new Category { Id = categoryJoyeriaId, Name = "Joyería", Slug = "joyeria", Description = "Collares, aretes y pulseras hechas a mano", ImageUrl = "https://example.com/jewelry.jpg" },
            new Category { Id = categoryTextilesId, Name = "Textiles", Slug = "textiles", Description = "Mochilas, hamacas y tejidos tradicionales", ImageUrl = "https://example.com/textiles.jpg" },
            new Category { Id = categoryCesteriaId, Name = "Cestería", Slug = "cesteria", Description = "Canastos y objetos decorativos en fibras naturales", ImageUrl = "https://example.com/baskets.jpg" },
            new Category { Id = categoryCeramicaId, Name = "Cerámica", Slug = "ceramica", Description = "Vasijas y figuras decorativas", ImageUrl = "https://example.com/ceramics.jpg" },
            new Category { Id = categoryTallasId, Name = "Tallas", Slug = "tallas", Description = "Figuras y máscaras talladas en madera", ImageUrl = "https://example.com/woodwork.jpg" }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // Seed Producers
        var producers = new List<Producer>
        {
            new Producer 
            { 
                Id = producer1Id,
                Name = "Artesanas de Quibdó", 
                Description = "Cooperativa de mujeres artesanas especializada en joyería tradicional", 
                Location = "Quibdó, Chocó", 
                ImageUrl = "https://example.com/producer1.jpg",
                Featured = true,
                FoundationYear = 2010,
                ContactPhone = "+57 300 123 4567",
                ContactEmail = "info@artesanasquibdo.com"
            },
            new Producer 
            { 
                Id = producer2Id,
                Name = "Tejedores del Pacífico", 
                Description = "Grupo familiar especializado en textiles y mochilas wayuu", 
                Location = "Istmina, Chocó", 
                ImageUrl = "https://example.com/producer2.jpg",
                Featured = true,
                FoundationYear = 2005,
                ContactPhone = "+57 310 987 6543",
                ContactEmail = "contacto@tejedorespcifico.com"            },
            new Producer 
            { 
                Id = producer3Id,
                Name = "Manos de Oro Chocoanas", 
                Description = "Artesanos especializados en cestería y objetos decorativos", 
                Location = "Condoto, Chocó", 
                ImageUrl = "https://example.com/producer3.jpg",
                Featured = false,
                FoundationYear = 2015,
                ContactPhone = "+57 320 555 1234"
            }
        };

        await context.Producers.AddRangeAsync(producers);
        await context.SaveChangesAsync();

        // Seed Products
        var products = new List<Product>
        {            // Jewelry
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Collar de Chaquiras Tradicional",
                Slug = "collar-chaquiras-tradicional",
                Description = "Hermoso collar elaborado con chaquiras de colores vibrantes, siguiendo patrones tradicionales de las comunidades afrocolombianas del Chocó.",
                Price = 85000m,
                DiscountedPrice = 75000m,
                ImageUrl = "https://example.com/collar1.jpg",
                Images = new List<string> { "https://example.com/collar1.jpg", "https://example.com/collar1-2.jpg" },
                Stock = 15,
                Featured = true,
                Rating = 4.8,
                Artisan = "María José Palacios",
                Origin = "Quibdó, Chocó",
                CategoryId = categoryJoyeriaId,
                ProducerId = producer1Id
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Aretes de Semillas Naturales",
                Slug = "aretes-semillas-naturales",
                Description = "Aretes elaborados con semillas de diferentes plantas nativas del Chocó, representando la conexión con la naturaleza.",
                Price = 45000m,
                ImageUrl = "https://example.com/aretes1.jpg",
                Images = new List<string> { "https://example.com/aretes1.jpg" },
                Stock = 25,
                Featured = false,
                Rating = 4.5,
                Artisan = "Carmen Lucía Mosquera",
                Origin = "Quibdó, Chocó",
                CategoryId = categoryJoyeriaId,
                ProducerId = producer1Id
            },
            
            // Textiles
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Mochila Wayuu Multicolor",
                Slug = "mochila-wayuu-multicolor",
                Description = "Mochila tejida a mano con técnicas ancestrales wayuu, con diseños geométricos en colores vibrantes.",
                Price = 120000m,
                ImageUrl = "https://example.com/mochila1.jpg",
                Images = new List<string> { "https://example.com/mochila1.jpg", "https://example.com/mochila1-2.jpg" },
                Stock = 8,
                Featured = true,
                Rating = 4.9,
                Artisan = "Rosa Elena Martínez",
                Origin = "Istmina, Chocó",
                CategoryId = categoryTextilesId,
                ProducerId = producer2Id
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Hamaca de Algodón",
                Slug = "hamaca-algodon",
                Description = "Hamaca tejida en algodón natural, perfecta para relajarse y disfrutar del clima tropical.",
                Price = 200000m,
                DiscountedPrice = 180000m,
                ImageUrl = "https://example.com/hamaca1.jpg",
                Images = new List<string> { "https://example.com/hamaca1.jpg" },
                Stock = 5,
                Featured = true,
                Rating = 4.7,
                Artisan = "José Manuel Rentería",
                Origin = "Istmina, Chocó",
                CategoryId = categoryTextilesId,
                ProducerId = producer2Id
            },
            
            // Basketry
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Canasto de Palma de Iraca",
                Slug = "canasto-palma-iraca",
                Description = "Canasto elaborado con fibras de palma de iraca, ideal para almacenamiento y decoración.",
                Price = 65000m,
                ImageUrl = "https://example.com/canasto1.jpg",
                Images = new List<string> { "https://example.com/canasto1.jpg" },
                Stock = 12,
                Featured = false,
                Rating = 4.4,
                Artisan = "Ana Milena Córdoba",
                Origin = "Condoto, Chocó",
                CategoryId = categoryCesteriaId,
                ProducerId = producer3Id
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        Console.WriteLine("Database seeded successfully!");
    }
}
