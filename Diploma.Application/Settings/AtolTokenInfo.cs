using System.Text.Json.Serialization;
using Diploma.Domain.Converters;

namespace Diploma.Application.Settings;

/// <summary>
/// Класс для хранения информации о токене для АТОЛ.
/// </summary>
public class AtolTokenInfo
{
    /// <summary>
    /// Токен для авторизации в системе АТОЛ.
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    /// <summary>
    /// Дата последнего получения токена в АТОЛ.
    /// </summary>
    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime LastLogin { get; set; }
}