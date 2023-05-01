using Diploma.DAL.Interfaces;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations;

public class SessionsPoolHandlerService : ISessionsPoolHandlerService
{
    private readonly IBaseRepository<ISessionHandlerService> _services;

    public SessionsPoolHandlerService(IBaseRepository<ISessionHandlerService> services)
    {
        _services = services;
    }
    
    public async IAsyncEnumerable<BaseResponse> AddNewBankOperationAsync(BankOperation operation)
    {
        ulong sessionId = operation.SessionId;
        if (_services.Contains(sessionId) == false)
        {
            _services.Add(sessionId, new SessionHandlerService());
        }
        var responses = _services.Get(sessionId).StartRecurringPayment(operation);
        await foreach (var recurringOperationResponse in responses)
        {
            if (recurringOperationResponse is SessionResponse)
            {
                _services.Remove(operation.SessionId);
            }
            yield return recurringOperationResponse;
        }
    }

    public IAsyncEnumerable<BaseResponse> GetOperationResponsesAsync(ulong sessionId)
    {
        throw new NotImplementedException();
        /*var responses = SessionHandlerServices[sessionId].HandleSessionAsync();
        await foreach (var recurringOperationResponse in responses)
        {
            yield return recurringOperationResponse;
        }*/
    }
}