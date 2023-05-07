using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;

namespace Diploma.Application.Interfaces;

public interface ISessionsPoolHandlerService
{
    public IAsyncEnumerable<BaseResponse> AddNewBankOperationAsync(BankOperationDto operation);
    public IAsyncEnumerable<BaseResponse> GetOperationResponsesAsync(ulong sessionId);
}