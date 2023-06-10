using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Responses;

/// <summary>
/// Класс хранения результатов рекуррентных платежей.
/// </summary>
public record RecurOperationResponse : BaseResponse
{
    /// <summary>
    /// Уникальный идентификатор записи в БД.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public ulong Id { get; set; }

    /// <summary>
    /// Сумма платежа.
    /// </summary>
    [JsonPropertyName("AMOUNT")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Идентификатор заказа.
    /// </summary>
    [JsonPropertyName("ORDER")]
    public ulong Order { get; set; }

    /// <summary>
    /// Номер авторизационного кода.
    /// </summary>
    [JsonPropertyName("RRN")]
    public ulong? Rrn { get; set; }

    /// <summary>
    /// Код ответа процессингового центра.
    /// </summary>
    [JsonPropertyName("RC")]
    public string? ResponseCode { get; set; }

    /// <summary>
    /// Описание ответа процессингового центра.
    /// </summary>
    [JsonPropertyName("RCTEXT")]
    public string? ResponseText { get; set; }

    /// <summary>
    /// Номер карты.
    /// </summary>
    [JsonPropertyName("CARD")]
    public string? CardNumber { get; set; }
}