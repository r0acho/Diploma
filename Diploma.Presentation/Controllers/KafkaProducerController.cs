using System.Net;
using System.Text.Json;
using Confluent.Kafka;
using Diploma.Application.Settings;
using Diploma.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Diploma.Presentation.Controllers;

/// <summary>
/// API для взаимодействия с брокером Kafka
/// </summary>
[ApiController]
[Route("[controller]")]
public class KafkaProducerController : Controller
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly IProducer<Null, string> _producer;

    public KafkaProducerController(IOptions<KafkaSettings> settings)
    {
        _kafkaSettings = settings.Value;
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers
        };
        _producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
    }


    /// <summary>
    /// Отправить сообщение в брокер
    /// </summary>
    /// <param name="receivedData">Тело сообщения с запросом на рекуррентный платеж</param>
    /// <returns>Status-code операции</returns>
    [HttpPost("RecurMessage")]
    public async Task<IActionResult> ProduceRecurMessage([FromBody] RecurringBankOperationDto receivedData)
    {
        if (!ModelState.IsValid) return StatusCode((int)HttpStatusCode.BadRequest, "Bad data request");
        try
        {
            var result = await _producer.ProduceAsync(_kafkaSettings.PaymentMessagesTopic,
                new Message<Null, string> { Value = JsonSerializer.Serialize(receivedData) });
            return Ok(
                $"Message '{result.Message.Value}' was sent to partition {result.TopicPartition.Partition} with offset {result.Offset}");
        }
        catch (ProduceException<Null, string> e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, $"Error producing message: {e.Error.Reason}");
        }
    }
}