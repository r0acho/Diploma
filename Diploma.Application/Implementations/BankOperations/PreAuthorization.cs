using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class PreAuthorization : Payment
{
    public PreAuthorization(PaymentModel model, byte[] secretKey) : base(model, secretKey)
    {
    }

    protected override TrType OperationType { get; } = TrType.PreAuthorization;
}