using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.SignalR;
using TrackerAPI.Hubs;

namespace TrackerAPI.Services
{

    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _topic = "shipment-tracking";
        private readonly string _groupId = "tracker-dashboard";
        private readonly string _bootstrapServers;

        // IHubContext allows us to talk to SignalR clients from OUTSIDE the Hub itself.
        private readonly IHubContext<TrackingHub> _hubContext;

        public KafkaConsumerService(IHubContext<TrackingHub> hubContext, IConfiguration configuration)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe(_topic);
            Console.WriteLine($"Kafka Background Consumer Started. Listening to {_topic}...");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        // 1. Consume from Kafka (blocks for 100ms then retries if empty)
                        var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(100));

                        if (consumeResult != null)
                        {
                            var message = consumeResult.Message.Value;
                            Console.WriteLine($"Broadcasting to UI: {message}");
                            
                            // 2. The Bridge: Send to ALL connected SignalR clients
                            // "ReceiveUpdate" is the method name the React frontend will listen for.
                            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", message, stoppingToken);
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error consuming: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}