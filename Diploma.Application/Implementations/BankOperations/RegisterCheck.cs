using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class RegisterCheck : BankOperationService
{
    public RegisterCheck(PaymentModel model, byte[] secretKey) : base(model, secretKey)
    {
    }

    protected override TrType OperationType { get; }
    protected override List<string> RequestKeys { get; }
    protected override List<string> PSignOrder { get; }
}