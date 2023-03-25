using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations;

public class SessionsPoolHandlerService : ISessionsPoolHandlerService
{
    public IDictionary<ulong, ISessionHandlerService> SessionHandlerServices { get; } =
        new Dictionary<ulong, ISessionHandlerService>();
    
    
    public async IAsyncEnumerable<BaseResponse> AddNewBankOperationAsync(BankOperation operation)
    {
        ulong sessionId = operation.SessionId;
        if (SessionHandlerServices.ContainsKey(sessionId) == false)
        {
            SessionHandlerServices.Add(sessionId, new SessionHandlerService());
        }
        var responses = SessionHandlerServices[sessionId].StartRecurringPayment(operation);
        await foreach (var recurringOperationResponse in responses)
        {
            if (recurringOperationResponse is SessionResponse)
            {
                SessionHandlerServices.Remove(operation.SessionId);
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