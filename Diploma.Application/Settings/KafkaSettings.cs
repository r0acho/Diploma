namespace Diploma.Application.Settings;

/// <summary>
/// Класс настроек для Kafka.
/// </summary>
public class KafkaSettings
{
    /// <summary>
    /// Список адресов серверов Kafka.
    /// </summary>
    public string? BootstrapServers { get; init; }

    /// <summary>
    /// Идентификатор группы потребителей.
    /// </summary>
    public string? GroupId { get; init; }

    /// <summary>
    /// Наименование топика для сообщений об оплате.
    /// </summary>
    public string? PaymentMessagesTopic { get; init; }

    /// <summary>
    /// Наименование топика для сообщений об операциях.
    /// </summary>
    public string? OperationMessagesTopic { get; init; }

    /// <summary>
    /// Наименование топика для сессионных сообщений.
    /// </summary>
    public string? SessionMessagesTopic { get; init; }
}