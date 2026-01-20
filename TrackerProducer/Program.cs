
using System.Text.Json;
using Confluent.Kafka;

var config = new ProducerConfig
{
    // Kafka broker address (how to connect to it)
    BootstrapServers = "localhost:9092"
};

//Kafka messages consist of a Key and a Value.
// Key is null since we don't need sorting
using var producer = new ProducerBuilder<Null, string>(config).Build();

Console.WriteLine("P: Driver Producer connected. Sending coordinates...");

// Simulate a simple route
double lat = 40.7128;
double lon = -74.0060;

while (true)
{
    //simulate moving slightly
    lat += 0.0001;
    lon += 0.0001;

    var location = new
    {
        DriverId = 1,
        Lat = lat,
        Lon = lon,
        TimeStamp = DateTime.UtcNow
    };

    string messageValue = JsonSerializer.Serialize(location);

    try
    {
        //Send to "test-topic"
        var deliveryReport = await producer.ProduceAsync("test-topic", 
            new Message<Null, string> { Value = messageValue });

        Console.WriteLine($"P: Sent '{messageValue}' to partition: {deliveryReport.Partition}, offset: {deliveryReport.Offset}");

    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"P: Delivery failed: {e.Error.Reason}");
    }

    Thread.Sleep(2000); // Send every 2 seconds
}