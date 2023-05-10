using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations.BankOperations;

public class CheckCard : Payment
{
    public CheckCard(PaymentModel model, BankSettings bankSettings) : base(model, bankSettings)
    {
    }

    protected override TrType OperationType { get; } = TrType.CheckCard;

    private bool IsCardMir()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Для операции "Проверка карты" сумму операции следует установить равной 0 (описание API банка)
    /// </summary>
    protected override void ChangeModelFieldsByInheritMembers()
    {
        SendingModel["AMOUNT"] = "0";
    }
}