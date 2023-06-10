using System.Text.Json.Serialization;

namespace Diploma.Domain.Entities.Atol;

/// <summary>
/// Класс, представляющий сервис, через который отправляется фискальный чек в формате АТОЛ.
/// </summary>
public class Service
{
    /// <summary>
    /// URL для получения ответа от сервиса обработки чеков.
    /// </summary>
    [JsonPropertyName("callback_url")]
    public string CallbackUrl { get; set; }
}