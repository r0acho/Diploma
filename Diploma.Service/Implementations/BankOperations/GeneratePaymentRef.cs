namespace Diploma.Service.Implementations.BankOperations;

public class GeneratePaymentRef : Payment
{
    protected override List<string> RequestKeys { get; } = new()
    {
        "AMOUNT", "CURRENCY", "DESC", "TERMINAL", "TRTYPE",
        "BACKREF", "ORDER", "EMAIL", "MERCHANT_NOTIFY_EMAIL",
        "P_SIGN"
    };

    protected override List<string> PSignOrder { get; } = new()
    {
        "AMOUNT", "CURRENCY", "TERMINAL", "TRTYPE", "BACKREF", "ORDER"
    };
}