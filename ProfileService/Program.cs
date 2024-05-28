using ProfileServiceApp.BackgroundServices;
using RabbitMQ.Client;
using MongoDB.Driver;
using ProfileServiceApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton(provider =>
{
    //ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
    ConnectionFactory factory = new ConnectionFactory() { HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") };
    return factory.CreateConnection();
});

builder.Services.AddControllers();

// MongoDB Configuration
builder.Services.AddSingleton<IMongoClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    //var connectionString = "mongodb+srv://user7364605:8kgoQKCx8EwKx32I@teammatetinder.yp8q8nb.mongodb.net/?retryWrites=true&w=majority&appName=TeammateTinder";
    var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string cannot be null or empty.");
    }
    Console.WriteLine($"MongoDB Connection String: {connectionString}");
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<ProfileService>(); // Add this line to register ProfileService
builder.Services.AddControllers();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the RabbitMQBackgroundService
builder.Services.AddHostedService<RabbitMQBackgroundService>();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }

    await next();
});

app.MapControllers();
app.Run();
