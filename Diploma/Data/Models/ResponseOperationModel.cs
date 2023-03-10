namespace Diploma.Data.Models;

public record ResponseOperationModel
{
    public decimal OfdAmount { get; set; }
    public decimal BankAmount { get; set; }
    public DateTime DateOfOperation { get; set; }
    public string? CardNumber { get; set; }
    public decimal Difference { get; set; }
    public string? TextResponse { get; set; }
}