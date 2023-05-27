using System.Text.Json.Serialization;

namespace Diploma.Domain.Dto;

public record RecurringBankOperationDto : BankOperationDto
{
    [JsonPropertyName("Уникальный идентификатор операции на ПШ")]
    public string? IntRef { get; set; }

    [JsonPropertyName("Уникальный идентификатор операции")]
    public ulong Rrn { get; set; }
}