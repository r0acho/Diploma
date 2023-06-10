using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Confluent.Kafka;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Dto;
using Diploma.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations
{
    /// <summary>
    /// Реализация фонового сервиса для чтения сообщений из топика Kafka.
    /// </summary>
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly KafkaSettings _kafkaSettings;
        private readonly ILogger<KafkaConsumerService> _logger;

        private readonly JsonSerializerOptions _options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
            WriteIndented = true
        };

        private readonly IServiceScopeFactory _serviceScopeFactory;

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

        /// <summary>
        /// Метод, вызываемый при запуске фонового сервиса.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_kafkaSettings.PaymentMessagesTopic);
            _logger.LogInformation("Модуль обработки сообщений сконфигурирован и готов к работе");
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Ожидание следующего сообщения");
                    var result = _consumer.Consume(stoppingToken);
                    try
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var sessionHandlerService = scope.ServiceProvider.GetRequiredService<ISessionHandlerService>();
                        var paymentModelDto =
                            JsonSerializer.Deserialize<RecurringBankOperationDto>(result.Message.Value, _options)!;
                        _logger.LogInformation("Началась обработка платежа {Order}", paymentModelDto.Order);
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

        /// <summary>
        /// Метод освобождения ресурсов.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            _consumer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
