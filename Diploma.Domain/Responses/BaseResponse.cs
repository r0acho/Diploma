using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Responses;

public record BaseResponse
{
    private string _description = string.Empty;
    protected IdnMapping _idn = new();

    [JsonPropertyName("DESC")]
    public string Description
    {
        get => _description;
        set => _description = _idn.GetUnicode(value);
    }

    [JsonPropertyName("TIMESTAMP")] public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
}