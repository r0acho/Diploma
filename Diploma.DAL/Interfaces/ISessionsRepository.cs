using Diploma.Domain.Entities;
using Diploma.Domain.Response;

namespace Diploma.DAL.Interfaces;

public interface ISessionsRepository
{
    public IAsyncEnumerable<BaseResponse> AddOperationAndGetResponsesAsync(BankOperation operation);

}