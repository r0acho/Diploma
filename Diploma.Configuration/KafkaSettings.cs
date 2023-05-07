namespace Diploma.Configuration;

public class KafkaSettings
{
    public string? BootstrapServers { get; init; }
    public string? GroupId { get; init; }
    public string? PaymentMessagesTopic { get; init; }
    public string? OperationMessagesTopic { get; init; }
    public string? SessionMessagesTopic { get; init; }
}