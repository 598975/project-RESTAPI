using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace DAT250_REST.Messaging
{
    public class RabbitMqClient<T>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;



        public RabbitMqClient()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "myuser",
                Password = "secret"
            };
             _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task PublishMessageAsync(String message, string queueName)
        {
            if (message == null)
            {
                return;
            }
            _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(message);
            var properties = _channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            await Task.Run(() => _channel.BasicPublish(exchange: "vote-events-exchange", routingKey: "vote.event", basicProperties: properties, body: body));
        }

    }
}
