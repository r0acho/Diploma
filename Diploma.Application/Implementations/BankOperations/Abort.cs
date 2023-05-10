using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations.BankOperations;

public class Abort : Return
{
    public Abort(PaymentModel model, BankSettings bankSettings) : base(model, bankSettings)
    {
    }

    protected override TrType OperationType { get; } = TrType.Abort;
}