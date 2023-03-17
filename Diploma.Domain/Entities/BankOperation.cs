using Diploma.Domain.Enums;

namespace Diploma.Domain.Entities;

public class BankOperation
{
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
    

    public BankOperation(IDictionary<string?, object> inputModel)
    {
        //возможно падение в runtime при вызове ToString()
        WillSessionContinue = Convert.ToBoolean(inputModel["Сессия продолжается?"]);
        OperationType = (OperationType)Convert.ToInt32(inputModel["Тип операции"]);
        Amount = Convert.ToDecimal(inputModel["Сумма платежа, Р"]);
        Order = Convert.ToUInt64("Внутренний идентификатор платежа");
        Description = inputModel["Описание платежа"].ToString() ?? string.Empty;
        MerchantName = inputModel["Компания-владелец станции"].ToString() ?? string.Empty;
        ClientEmail = inputModel["Email клиента"].ToString() ?? string.Empty;
        MerchantEmail = inputModel["Email владельца станции"].ToString() ?? string.Empty;
        SessionId = Convert.ToUInt64(inputModel["Идентификатор резерва или зарядной сессии"]);
        ClientPhoneNumber = inputModel["Телефон клиента"].ToString() ?? string.Empty;
    }
        
}