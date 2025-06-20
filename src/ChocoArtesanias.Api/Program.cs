using ChocoArtesanias.Application;
using ChocoArtesanias.Infrastructure;
using ChocoArtesanias.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.

// Controllers
builder.Services.AddControllers();

// Infrastructure layer
builder.Services.AddInfrastructure(builder.Configuration);

// Application layer
builder.Services.AddApplication();

// Authentication
var keyString = builder.Configuration["TokenKey"] ?? throw new Exception("TokenKey no configurada");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString)),
            ValidateIssuer = true,
            ValidIssuer = "ChocoArtesanias",
            ValidateAudience = true,
            ValidAudience = "ChocoArtesanias-Users",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5) // More tolerant clock skew
        };
    });

// Authorization
builder.Services.AddAuthorization();
    
// Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS Policy
builder.Services.AddCors(options => {
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("AllowReactApp", policy => {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5000", "https://localhost:5001")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    }
    else
    {
        options.AddPolicy("AllowReactApp", policy => {
            policy.WithOrigins("http://localhost:5173") // Configure with production URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    }
});

var app = builder.Build();

// 2. Configure the HTTP request pipeline.

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try 
    {
        // Apply any pending migrations
        context.Database.Migrate();
        
        // Seed initial data
        await DbSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar la política de CORS
app.UseCors("AllowReactApp");

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
