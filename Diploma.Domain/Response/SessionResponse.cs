using System.Text.Json.Serialization;

namespace Diploma.Domain.Response;

public class SessionResponse : BaseResponse
{
    public decimal TouchAmount { get; set; }
    public decimal BankAmount { get; set; }
    public decimal Difference() => TouchAmount - BankAmount;
    [JsonPropertyName("AMOUNT")]
    public decimal Amount { get; set; }
    [JsonPropertyName("RC")]
    public string? ResponseCode { get; set; }
    [JsonPropertyName("RCTEXT")]
    public string? ResponseText { get; set; }
    [JsonPropertyName("CARD")]
    public string? CardNumber { get; set; }
}