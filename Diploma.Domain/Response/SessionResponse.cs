namespace Diploma.Domain.Response;

public class SessionResponse : BaseResponse
{
    public decimal TouchAmount { get; set; }
    public decimal BankAmount { get; set; }
    public DateTime DateOfOperation { get; set; } = DateTime.Today;
    public string? CardNumber { get; set; }
    public decimal Difference() => TouchAmount - BankAmount;
    public string? TextResponse { get; set; }
}