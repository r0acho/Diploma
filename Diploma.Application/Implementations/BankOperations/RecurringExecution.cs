using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class RecurringExecution : Payment
{
    public RecurringExecution(PaymentModel model, byte[] secretKey) : base(model, secretKey)
    {
    }

    protected override TrType OperationType { get; } = TrType.Recurring;

    protected override List<string> RequestKeys { get; } = new()
    {
        "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
        "MERCHANT", "EMAIL", "TIMESTAMP", "NONCE", "BACKREF", "NOTIFY_URL", "INT_REF", "RECUR_REF"
    };

    protected override void ChangeModelFieldsByInheritMembers()
    {
        if (_model is not RecurringPaymentModel recurringBankOperation)
            throw new ArgumentException("Нет данных для проведения рекуррентного платежа");
        SendingModel["INT_REF"] = recurringBankOperation.IntRef!;
        SendingModel["RECUR_REF"] = recurringBankOperation.Rrn.ToString();
    }
}