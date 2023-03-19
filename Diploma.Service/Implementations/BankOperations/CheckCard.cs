using Diploma.Service.Enums;

namespace Diploma.Service.Implementations.BankOperations;

public class CheckCard : Payment
{
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
        _model["AMOUNT"] = "0";
    }
}