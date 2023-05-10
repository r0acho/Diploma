using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface ISessionsPoolHandlerService
{
    IAsyncEnumerable<BaseResponse> AddNewBankOperationAsync(BankOperationDto operation);
}