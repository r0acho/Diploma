using Diploma.DAL.Interfaces;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Service.Implementations;
using Diploma.Service.Interfaces;

namespace Diploma.DAL.Implementations;

public class SessionsRepository : ISessionsRepository
{
    private readonly Dictionary<ulong, ISessionHandlerService> _sessionHandlerServices = new();
    
    public async IAsyncEnumerable<BaseResponse> AddOperationAndGetResponsesAsync(BankOperation operation)
    {
        ulong sessionId = operation.SessionId;
        if (_sessionHandlerServices.ContainsKey(sessionId) == false)
        {
            _sessionHandlerServices.Add(sessionId, new SessionHandlerService());
        }
        var responses = _sessionHandlerServices[sessionId].StartRecurringPayment(operation);
        await foreach (var recurringOperationResponse in responses)
        {
            if (recurringOperationResponse is SessionResponse)
            {
                _sessionHandlerServices.Remove(operation.SessionId);
            }
            yield return recurringOperationResponse;
        }
    }
}