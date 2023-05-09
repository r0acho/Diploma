using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class RegisterCheck : BankOperationService
{
    public RegisterCheck(PaymentModel model, BankSettings bankSettings) : base(model, bankSettings)
    {
    }

    protected override TrType OperationType { get; }
    protected override List<string> RequestKeys { get; }
    protected override List<string> PSignOrder { get; }
}