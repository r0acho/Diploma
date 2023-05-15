using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Diploma.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diploma.Domain.Responses;

public record SessionResponse : BaseResponse
{
    [Key]
    public ulong Id { get; set; }
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
    [EnumDataType(typeof(SessionStatus))]
    public SessionStatus SessionStatus { get; set; }
}