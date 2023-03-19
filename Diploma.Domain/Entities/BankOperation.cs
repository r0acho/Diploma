using Diploma.Domain.Enums;

namespace Diploma.Domain.Entities;

public class BankOperation
{
    public BankOperation(IDictionary<string, string> inputModel)
    {
        //возможно падение в runtime при вызове ToString()
        WillSessionContinue = Convert.ToBoolean(inputModel["Сессия продолжается?"]);
        OperationType = (OperationType)Convert.ToInt32(inputModel["Тип операции"]);
        Amount = Convert.ToDecimal(inputModel["Сумма платежа, Р"]);
        Order = Convert.ToUInt64(inputModel["Внутренний идентификатор платежа"]);
        Description = inputModel["Описание платежа"] ?? string.Empty;
        MerchantName = inputModel["Компания-владелец станции"] ?? string.Empty;
        ClientEmail = inputModel["Email клиента"] ?? string.Empty;
        MerchantEmail = inputModel["Email владельца станции"] ?? string.Empty;
        SessionId = Convert.ToUInt64(inputModel["Идентификатор резерва или зарядной сессии"]);
        ClientPhoneNumber = inputModel["Телефон клиента"] ?? string.Empty;
    }

    public decimal Amount { get; }
    public bool WillSessionContinue { get; }
    public OperationType OperationType { get; }
    public ulong Order { get; }
    public string Description { get; }
    public string MerchantName { get; }
    public string ClientEmail { get; }
    public string MerchantEmail { get; }
    public ulong SessionId { get; }
    public string ClientPhoneNumber { get; }
}