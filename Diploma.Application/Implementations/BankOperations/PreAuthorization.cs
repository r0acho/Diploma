using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Application.Implementations.BankOperations;

public class PreAuthorization : Payment
{
    public PreAuthorization(PaymentModel model, BankSettings bankSettings) : base(model, bankSettings)
    {
    }

    protected override TrType OperationType { get; } = TrType.PreAuthorization;
}