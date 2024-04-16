using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace ProfileServiceApp.BackgroundServices
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly IConnection _rabbitMQConnection;
        private readonly IModel _profileChannel;
        private readonly string _profileChannelName = "profile";

        public RabbitMQBackgroundService(IConnection rabbitMQConnection)
        {
            _rabbitMQConnection = rabbitMQConnection;
            _profileChannel = _rabbitMQConnection.CreateModel();
            _profileChannel.QueueDeclare(queue: _profileChannelName, exclusive: false, autoDelete: false);
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
            };
            _profileChannel.BasicConsume(
                queue: _profileChannelName,
                autoAck: true,
                consumer: consumer);

            await Task.CompletedTask;
        }
    }
}
