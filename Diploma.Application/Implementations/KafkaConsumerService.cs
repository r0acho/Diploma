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
        //TODO
        _consumer.Subscribe(_kafkaSettings.PaymentMessagesTopic);
        await Task.Run( () =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var sessionsPoolHandlerService = scope.ServiceProvider.GetService<ISessionsPoolHandlerService>()! ??
                                             throw new InvalidOperationException("Невозможно создать контекст пула сессий");
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                try
                {
                    var paymentModelDto =
                        JsonSerializer.Deserialize<RecurringBankOperationDto>(result.Message.Value, _options)!;
                    var responses = sessionsPoolHandlerService.AddNewBankOperationAsync(paymentModelDto);
                    _logger.LogInformation($"НАЧАЛАСЬ ОБРАБОТКА ПЛАТЕЖА {paymentModelDto.Order}");
                    UpdateDatabaseByResponses(responses);
                }
                catch (JsonException)
                {
                    _logger.LogError("Ошибка конвертации JSON-объекта");
                }
                catch
                {
                    _logger.LogError("Ошибка считывания сообщения из топика Kafka");
                }

            }
        }, stoppingToken);
    }

    private async void UpdateDatabaseByResponses(IAsyncEnumerable<BaseResponse> responses)
    {
        var sessionResponsesRepository = _serviceScopeFactory.CreateScope().ServiceProvider
            .GetService<IResponsesRepository<SessionResponse>>() ?? throw new InvalidOperationException("Невозможно создать контекст таблицы сессий базы данных");
        var recurResponsesRepository = _serviceScopeFactory.CreateScope().ServiceProvider
            .GetService<IResponsesRepository<RecurOperationResponse>>()?? throw new InvalidOperationException("Невозможно создать контекст таблицы рекуррентных платежей базы данных");
        var fiscalResponsesRepository = _serviceScopeFactory.CreateScope().ServiceProvider
            .GetService<IResponsesRepository<FiscalPaymentResponse>>()?? throw new InvalidOperationException("Невозможно создать контекст таблицы чеков базы данных");

        await foreach (var response in responses)
        {
            if (response is SessionResponse sessionResponse)
            {
                await sessionResponsesRepository.Create(sessionResponse);
            }
            else if (response is RecurOperationResponse recurOperationResponse)
            {
                await recurResponsesRepository.Create(recurOperationResponse);
            }
            else if (response is FiscalPaymentResponse fiscalOperationResponse)
            {
                await fiscalResponsesRepository.Create(fiscalOperationResponse);
            }
        }
    }
    
    public override void Dispose()
    {
        base.Dispose();
        _consumer.Dispose();
        GC.SuppressFinalize(this);
    }
}