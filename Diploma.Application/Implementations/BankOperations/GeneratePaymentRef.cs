using Diploma.Domain.Entities;

namespace Diploma.Application.Implementations.BankOperations;

public class GeneratePaymentRef : Payment
{
    public GeneratePaymentRef(PaymentModel model, byte[] secretKey) : base(model, secretKey)
    {
    }

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