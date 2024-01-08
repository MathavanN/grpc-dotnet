using BasketService.API.Infrastructure.Interfaces;
using BasketService.API.Infrastructure.Repositories;
using BasketService.API.Services;
using Carter;
using ProductServiceApi;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddScoped<IProductService, ProductService>()
    .AddGrpcClient<ProductServiceGrpc.ProductServiceGrpcClient>((services, options) =>
    {
        options.Address = new Uri("https://localhost:7069");
    });

builder.Services.AddSingleton(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddTransient<IBasketRepository, RedisBasketRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseHttpsRedirection();

app.Run();

