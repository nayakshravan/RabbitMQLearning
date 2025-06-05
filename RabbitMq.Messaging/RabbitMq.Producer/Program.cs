using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "message-exchange",
    durable: true,
    autoDelete: false,
    type:ExchangeType.Fanout);

await Task.Delay(10000);

for(int i= 1;i<=10; i++)
{
    var message = "Message "+i+" "+DateTime.UtcNow;
    var body=Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(
        exchange: "message-exchange",
        routingKey: string.Empty,
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body);

    Console.WriteLine("Sent " + message);

    await Task.Delay(2000);
}