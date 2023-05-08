using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Diploma.Application.Interfaces;
using System.Text.Json;
using Diploma.Domain.Dto;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Diploma.Application.Settings;
using Microsoft.Extensions.Logging;
using Diploma.Domain.Entities;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Application.Implementations;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ISessionsPoolHandlerService _sessionsPoolHandlerService;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly JsonSerializerOptions _options = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
        WriteIndented = true
    };

    public KafkaConsumerService(
        IConfiguration config, 
        ISessionsPoolHandlerService sessionsPoolHandlerService,
        ILogger<KafkaConsumerService> logger)
    {
        _logger = logger;
        _sessionsPoolHandlerService = sessionsPoolHandlerService;
        _kafkaSettings = config.GetSection("Kafka").Get<KafkaSettings>()!;
        var kafkaConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _consumer = new ConsumerBuilder<Ignore, string>(kafkaConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //TODO
        _consumer.Subscribe(_kafkaSettings.PaymentMessagesTopic);
        await Task.Run(async () =>
        {
            using (_consumer)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(stoppingToken);
                    try
                    {
                        var paymentModelDto =
                            JsonSerializer.Deserialize<RecurringBankOperationDto>(result.Message.Value, _options)!;
                        var paymentModel = new RecurringPaymentModel();
                        paymentModel.SetFromDtoModel(paymentModelDto);
                        var responses =  _sessionsPoolHandlerService.AddNewBankOperationAsync(paymentModelDto);
                        await foreach (var response in responses)
                        {
                            _logger.LogInformation(
                                $"{JsonSerializer.Serialize(response, _options)}");
                        }
                        
                    }
                    catch (JsonException)
                    {
                        _logger.LogError("Ошибка конвертации JSON-объекта");
                    }
                    catch
                    {
                        _logger.LogError("Неизвестная ошибка при обработке сообщения из Kafka");
                    }

                }
            }
        }, stoppingToken);


    }
}