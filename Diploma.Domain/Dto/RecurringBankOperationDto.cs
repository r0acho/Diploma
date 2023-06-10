using System.Text.Json.Serialization;

namespace Diploma.Domain.Dto;

/// <summary>
/// Класс DTO для повторяющейся банковской операции.
/// </summary>
public record RecurringBankOperationDto : BankOperationDto
{
    /// <summary>
    /// Уникальный идентификатор операции на ПШ.
    /// </summary>
    [JsonPropertyName("Уникальный идентификатор операции на ПШ")]
    public string? IntRef { get; set; }

    /// <summary>
    /// Уникальный идентификатор операции.
    /// </summary>
    [JsonPropertyName("Уникальный идентификатор операции")]
    public ulong Rrn { get; set; }
}