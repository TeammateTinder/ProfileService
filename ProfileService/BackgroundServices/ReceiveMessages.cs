using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using ProfileServiceApp.Services;
using MongoDB.Driver;

namespace ProfileServiceApp.BackgroundServices
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly IConnection _rabbitMQConnection;
        private readonly IModel _profileChannel;
        private readonly string _profileChannelName = "profile";
        private readonly ProfileService _profileService; 

        public RabbitMQBackgroundService(IConnection rabbitMQConnection, IMongoClient mongoClient)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _profileChannel = _rabbitMQConnection.CreateModel();
            _profileChannel.QueueDeclare(queue: _profileChannelName, exclusive: false, autoDelete: false);
            _profileService = new ProfileService(mongoClient);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(_profileChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received: {message}");

                // Attempt to parse the message into swiperID and id2
                if (message.Contains(":") && message.Split(':').Length == 3) 
                {
                    string[] parts = message.Split(':');
                    // ID:ID:y / ID:ID:n
                    if (int.TryParse(parts[0], out int swiperID) && int.TryParse(parts[1], out int swipedID) && (parts[2] == "y" || parts[2] == "n"))
                    {
                        Console.WriteLine($" [x] Received: SwiperID={swiperID}, ID2={swipedID}, Flag={parts[2]}");
                        
                        if (parts[2] == "y")
                        {
                            _profileService.AddIDToSwipedYes(swiperID, swipedID);
                        }
                        else if (parts[2] == "n")
                        {
                            _profileService.AddIDToSwipedNo(swiperID, swipedID);
                        }
                    }
                    else
                    {
                        Console.WriteLine(" [x] Error: Invalid message format or flag");
                    }
                }
                else
                {
                    Console.WriteLine(" [x] Error: Invalid message format");
                }

            };
            _profileChannel.BasicConsume(
                queue: _profileChannelName,
                autoAck: true,
                consumer: consumer);

            await Task.CompletedTask;
        }
    }
}
