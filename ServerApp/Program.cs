using ServerApp.Models;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// === CORS ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// === IN-MEMORY CACHING (Performance Optimization) ===
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseCors("DevPolicy");

// === INJECT CACHE ===
var cache = app.Services.GetRequiredService<IMemoryCache>();
var cacheKey = "productList";
var cacheExpiry = TimeSpan.FromMinutes(5);

// === /api/productlist – CACHED RESPONSE ===
app.MapGet("/api/productlist", () =>
{
    if (!cache.TryGetValue(cacheKey, out ProductDto[]? products))
    {
        products = new[]
        {
            new ProductDto
            {
                Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25,
                Category = new CategoryDto { Id = 101, Name = "Electronics" }
            },
            new ProductDto
            {
                Id = 2, Name = "Headphones", Price = 50.00, Stock = 100,
                Category = new CategoryDto { Id = 102, Name = "Accessories" }
            },
            new ProductDto
            {
                Id = 3, Name = "USB Cable", Price = 15.99, Stock = 200,
                Category = new CategoryDto { Id = 102, Name = "Accessories" }
            }
        };

        // Cache for 5 minutes
        cache.Set(cacheKey, products, cacheExpiry);
    }

    return products;
});

// Copilot helped generate this caching pattern
// It reduces redundant object creation on every request

app.Run();