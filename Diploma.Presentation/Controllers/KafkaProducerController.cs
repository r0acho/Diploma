using System.Net;
using Diploma.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

using Confluent.Kafka;

[ApiController]
[Route("[controller]")]
public class KafkaProducerController : Controller
{
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaSettings _kafkaSettings;

    public KafkaProducerController(IConfiguration config)
    {
        _kafkaSettings = config.GetSection("Kafka").Get<KafkaSettings>()!;
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers
        };
        _producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
    }

    [HttpPost]
    public async Task<IActionResult> ProduceMessage(string message)
    {
        try
        {
            var result = await _producer.ProduceAsync(_kafkaSettings.PaymentMessagesTopic, new Message<Null, string> { Value = message });
            return Ok($"Message '{result.Message.Value}' was sent to partition {result.TopicPartition.Partition} with offset {result.Offset}");
        }
        catch (ProduceException<Null, string> e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, $"Error producing message: {e.Error.Reason}");
        }
    }
}