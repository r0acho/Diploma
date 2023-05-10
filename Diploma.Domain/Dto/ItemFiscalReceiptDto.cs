
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Diploma.Domain.Dto
{
    
    public class ItemFiscalReceiptDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonIgnore]
        public ulong Id { get; set; }
        public string? Description { get; init; }  
        public uint Price { get; init; }
        public decimal QtyDecimal { get; init; }
        public ushort TaxId { get; init; }
        public uint PayAttribute { get; init; }
    }
}
