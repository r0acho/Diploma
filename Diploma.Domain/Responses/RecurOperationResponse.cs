using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Responses;

public record RecurOperationResponse : BaseResponse
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public ulong Id { get; set; }
    [JsonPropertyName("AMOUNT")] public decimal Amount { get; set; }
    
    [JsonPropertyName("ORDER")] public ulong Order { get; set; }

    [JsonPropertyName("RRN")] public ulong? Rrn { get; set; }

    [JsonPropertyName("RC")] public string? ResponseCode { get; set; }

    [JsonPropertyName("RCTEXT")] public string? ResponseText { get; set; }

    [JsonPropertyName("CARD")] public string? CardNumber { get; set; }
}