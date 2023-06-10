using System.Text.Json.Serialization;
using Diploma.Domain.Enums;

namespace Diploma.Domain.Dto;

public record BankOperationDto
{
    [JsonPropertyName("Сумма платежа, Р")] public decimal Amount { get; set; }

    [JsonPropertyName("Внутренний идентификатор платежа")]
    public ulong Order { get; set; }

    [JsonPropertyName("Описание платежа")] public string? Description { get; set; }

    [JsonPropertyName("Компания-владелец станции")]
    public string? MerchantName { get; set; }

    [JsonPropertyName("Email клиента")] public string? ClientEmail { get; set; }

    [JsonPropertyName("Email владельца станции")]
    public string? MerchantEmail { get; set; }

    [JsonPropertyName("Идентификатор резерва или зарядной сессии")]
    public ulong SessionId { get; set; }

    [JsonPropertyName("Номер ТСП")] public string? MerchantId { get; set; }

    [JsonPropertyName("Номер устройства")] public ulong TerminalId { get; set; }
}