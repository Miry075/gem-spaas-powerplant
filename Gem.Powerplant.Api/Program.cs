using Gem.Powerplant.Application.Interfaces;
using Gem.Powerplant.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IDispatchService, DispatchService>();
builder.Services.AddScoped<IProductionService, ProductionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger UI must point to the OpenAPI endpoint
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

