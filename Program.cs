using ss_inventory_microservice.Data;
using ss_inventory_microservice.Repositories;
using MongoDB.Driver;
using ss_inventory_microservice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(mongoConnectionString));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<InventoryRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Service Adapter setup
builder.Services.AddHttpClient<ServiceAdapter>(client =>
{
    //  Other microservice URL
    client.BaseAddress = new Uri("https://other-microservice-url.com/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();