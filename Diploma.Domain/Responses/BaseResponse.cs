using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Responses;

/// <summary>
/// Базовый класс для ответов от сервисов.
/// </summary>
public record BaseResponse
{
    private string _description = string.Empty;
    protected IdnMapping _idn = new();

    /// <summary>
    /// Описание ответа.
    /// </summary>
    [JsonPropertyName("DESC")]
    public string Description
    {
        get => _description;
        set => _description = _idn.GetUnicode(value);
    }

    /// <summary>
    /// Временная метка ответа в формате yyyyMMddHHmmss.
    /// </summary>
    [JsonPropertyName("TIMESTAMP")]
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

    /// <summary>
    /// Код статуса HTTP-ответа.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
}