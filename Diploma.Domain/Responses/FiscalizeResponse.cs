using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Responses;

public record FiscalizeResponse
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public ulong Id { get; set; }
    
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("error")]
    public string? ErrorJson;
    
    [JsonPropertyName("timestamp")]
    [JsonConverter(typeof(Diploma.Domain.Converters.DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    public ulong SessionId { get; set; }
}