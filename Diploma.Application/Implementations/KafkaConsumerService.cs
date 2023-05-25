using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Diploma.Application.Interfaces;
using System.Text.Json;
using Diploma.Domain.Dto;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Diploma.Application.Settings;
using Diploma.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly KafkaSettings _kafkaSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly JsonSerializerOptions _options = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
        WriteIndented = true
    };
    
    public KafkaConsumerService(
        IOptions<KafkaSettings> settings, 
        IServiceScopeFactory serviceScopeFactory,
        ILogger<KafkaConsumerService> logger)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _kafkaSettings = settings.Value;
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
        _consumer.Subscribe(_kafkaSettings.PaymentMessagesTopic);
        await Task.Run( async() =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var sessionHandlerService = scope.ServiceProvider.GetRequiredService<ISessionHandlerService>();
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                try
                {
                    var paymentModelDto =
                        JsonSerializer.Deserialize<RecurringBankOperationDto>(result.Message.Value, _options)!;
                    _logger.LogInformation("НАЧАЛАСЬ ОБРАБОТКА ПЛАТЕЖА {Order}", paymentModelDto.Order);
                    await sessionHandlerService.StartRecurringPayment(paymentModelDto.SessionId,
                        paymentModelDto.GetModelFromDto());
                }
                catch (JsonException)
                {
                    _logger.LogError("Ошибка конвертации JSON-объекта");
                }
                catch 
                {
                    _logger.LogError("Ошибка обработки сообщения из топика Kafka");
                }

            }
        }, stoppingToken);
    }
    
    public override void Dispose()
    {
        base.Dispose();
        _consumer.Dispose();
        GC.SuppressFinalize(this);
    }
}