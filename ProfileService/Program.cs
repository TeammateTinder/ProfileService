using Microsoft.AspNetCore.Connections;
using ProfileServiceApp.BackgroundServices;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProfileServiceApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton(provider =>
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    return factory.CreateConnection();
});

// MongoDB Configuration
builder.Services.AddSingleton<IMongoClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("MongoDB");
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<ProfileService>(); // Add this line to register ProfileService

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the RabbitMQBackgroundService
builder.Services.AddHostedService<RabbitMQBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
