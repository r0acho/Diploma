using Diploma.Domain.Entities;
using Diploma.Domain.Response;

namespace Diploma.Service.Interfaces;

public interface ISessionsPoolHandlerService
{
    internal IDictionary<ulong, ISessionHandlerService> SessionHandlerServices { get; }
    public void AddNewBankOperationAsync(BankOperation operation);
    public IAsyncEnumerable<BaseResponse> GetOperationResponsesAsync(ulong sessionId);
}