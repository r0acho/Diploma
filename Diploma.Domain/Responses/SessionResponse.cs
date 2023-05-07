using System.Text.Json.Serialization;

namespace Diploma.Domain.Responses;

public class SessionResponse : BaseResponse
{
    public decimal TouchAmount { get; set; }
    public decimal BankAmount { get; set; }
    [JsonPropertyName("Difference")]
    public decimal Difference => TouchAmount - BankAmount;

    [JsonPropertyName("RC")]
    public string? ResponseCode { get; set; }
    [JsonPropertyName("RCTEXT")]
    public string? ResponseText { get; set; }
    [JsonPropertyName("CARD")]
    public string? CardNumber { get; set; }
}