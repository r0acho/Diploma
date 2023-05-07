using Diploma.Domain.Entities;
using Diploma.Domain.Response;

namespace Diploma.Application.Interfaces;

public interface ISessionsPoolHandlerService
{
    public IAsyncEnumerable<BaseResponse> AddNewBankOperationAsync(BankOperation operation);
    public IAsyncEnumerable<BaseResponse> GetOperationResponsesAsync(ulong sessionId);
}