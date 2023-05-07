using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities;

public class RecurringBankOperation : BankOperation
{
    [JsonPropertyName("Уникальный идентификатор операции на ПШ")]
    public string? IntRef { get; set; }
    
    [JsonPropertyName("Уникальный идентификатор операции")]
    public ulong Rrn { get; set; }
}