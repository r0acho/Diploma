using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations;

public class SessionsPoolHandlerService : ISessionsPoolHandlerService
{
    public IDictionary<ulong, ISessionHandlerService> SessionHandlerServices { get; } =
        new Dictionary<ulong, ISessionHandlerService>();
    
    public void AddNewBankOperationAsync(BankOperation operation)
    {
        ulong sessionId = operation.SessionId;
        if (SessionHandlerServices.ContainsKey(sessionId) == false)
        {
            SessionHandlerServices.Add(sessionId, new SessionHandlerService());
        }
        SessionHandlerServices[sessionId].Operations.Add(operation);
    }

    public async IAsyncEnumerable<BaseResponse> GetOperationResponsesAsync(ulong sessionId)
    {
        var responses = SessionHandlerServices[sessionId].HandleSessionAsync();
        await foreach (var recurringOperationResponse in responses)
        {
            yield return recurringOperationResponse;
        }
    }
}