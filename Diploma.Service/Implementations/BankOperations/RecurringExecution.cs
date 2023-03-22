using Diploma.Domain.Entities;
using Diploma.Service.Enums;

namespace Diploma.Service.Implementations.BankOperations;

public class RecurringExecution : Payment
{
    protected override TrType OperationType { get; } = TrType.Recurring;

    protected override List<string> RequestKeys { get; init; } = new()
    {
        "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
        "MERCHANT", "EMAIL", "TIMESTAMP", "NONCE", "BACKREF", "NOTIFY_URL", "INT_REF", "RECUR_REF"
    };

    protected override void ChangeModelFieldsByInheritMembers()
    {
        if (CurrentBankOperation is not RecurringBankOperation recurringBankOperation)
            throw new ArgumentException("Нет данных для проведения рекуррентного платежа");
        SendingModel["INT_REF"] = recurringBankOperation.IntRef!;
        SendingModel["RECUR_REF"] = recurringBankOperation.Rrn.ToString();
    }
}