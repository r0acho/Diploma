using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class Abort : Return
{
    public Abort(PaymentModel model, byte[] secretKey) : base(model, secretKey)
    {
    }

    protected override TrType OperationType { get; } = TrType.Abort;
}