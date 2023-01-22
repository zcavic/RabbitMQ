using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

class EmitLogTopic
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "yomingo.models.events.clientdeletedevent",
                                    type: "topic");

            var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
            
            var message = new Message() { ClientId = 1 };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: "yomingo.models.events.clientdeletedevent",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
        }
    }
}


    public class Message
{
    public int ClientId { get; set; }
}