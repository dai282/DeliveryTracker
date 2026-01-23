using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace TrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
//api/shipment
public class ShipmentsController : ControllerBase
{
    private readonly IProducer<Null, string> _producer;

    public ShipmentsController(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateLocation([FromBody] ShipmentUpdate update)
    {
        // 1. Serialize the C# object to a JSON string
        var jsonString = JsonSerializer.Serialize(update);

        try
        {
            // 2. Wrap it in a Kafka Message
            var message = new Message<Null, string>
            {
                Value = jsonString
            };

            // 3. Send to Kafka (topic: "shipment-tracking")
            // We await this to ensure Kafka received it before telling the HTTP client "OK"
            var deliveryResult = await _producer.ProduceAsync("shipment-tracking", message);

            return Ok(new
            {
                Status = "Sent to Kafka",
                Offset = deliveryResult.Offset.Value,
                Partition = deliveryResult.Partition.Value
            });

        }
        catch (ProduceException<Null, string> e)
        {
            // 1. Log the specific Kafka error
            Console.WriteLine($"Kafka Delivery Failed: {e.Error.Reason}");

            return StatusCode(500, "Failed to deliver message to Kafka.");
        }
        catch (Exception ex)
        {
            // 2. Log any other error
            Console.WriteLine($"Error: {ex.Message}");

            return StatusCode(500, "An error occurred while processing the request.");
        }

    }

}