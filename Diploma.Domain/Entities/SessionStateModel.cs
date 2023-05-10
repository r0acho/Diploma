using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Diploma.Domain.Dto;
using Diploma.Domain.Enums;


namespace Diploma.Domain.Entities;

public record SessionStateModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public ulong Id { get; set; }

    public List<ItemFiscalReceiptDto> Items { get; set; } = new();
    
    [EnumDataType(typeof(SessionStatus))]
    public SessionStatus Status { get; set; } = SessionStatus.InProgress;
    public decimal SumOfSessionsByBank { get; set; } = 0;
    public decimal SumOfSessionsByTouch { get; set; } = 0;
    

}