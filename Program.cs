using ss_inventory_microservice.Data;
using ss_inventory_microservice.Repositories;
using MongoDB.Driver;
using ss_inventory_microservice;

var builder = WebApplication.CreateBuilder(args);

// MongoDB Connection
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<MongoDbContext>();

// RabbitMQ Connection
builder.Services.AddScoped<InventoryRepository>();
builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
builder.Services.AddHostedService<OrderReceivedConsumer>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add swagger documentation for Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();