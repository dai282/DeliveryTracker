using System.Runtime.Serialization;
using Confluent.Kafka;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "tracker-dashboard-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

consumer.Subscribe("shipment-tracking");

Console.WriteLine("C: Dashboard Consumer listening...");

try
{
    while (true)
    {
        var consumeResult = consumer.Consume(CancellationToken.None);
        Console.WriteLine($"C: Received update: {consumeResult.Message.Value}");
    }
}
catch (OperationCanceledException)
{
    consumer.Close();
}
