using Diploma.Domain.Entities;

namespace Diploma.Service.Interfaces;

public interface ISessionHandlerService
{
    bool CheckKeys(Dictionary<string, string> bankResponse);
    Task<string> GetJsonResult();
    Task<string> GetBankResponse(BankOperation bankOperation);
}