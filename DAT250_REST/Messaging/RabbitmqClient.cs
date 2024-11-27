﻿using Newtonsoft.Json;
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
            ConnectionFactory factory = new()
            {
                HostName = "host.docker.internal",
                UserName = "myuser",
                Password = "secret",
                Port = 5672
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task PublishMessageAsync(T message, string queueName)
        {
            if (message == null)
            {
                return;
            }
            _channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var messageJson = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            var body = Encoding.UTF8.GetBytes(messageJson);
            var properties = _channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            await Task.Run(() => _channel.BasicPublish(exchange: "vote-events-exchange", routingKey: "vote.event", basicProperties: properties, body: body));
        }

    }
}
