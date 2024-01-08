using Carter;
using Microsoft.EntityFrameworkCore;
using ProductService.API.Extensions;
using ProductService.API.Grpc;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddGrpc();
builder.Services.AddAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductServiceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.MigrateDbContext<ProductServiceContext>((context, services) =>
{
    ILogger<ProductContextSeed> logger = services.GetRequiredService<ILogger<ProductContextSeed>>();

    new ProductContextSeed()
        .SeedAsync(context, app.Environment, logger)
        .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapGrpcService<GrpcProductService>();

app.Run();

