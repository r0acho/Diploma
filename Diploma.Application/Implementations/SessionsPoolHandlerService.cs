using Diploma.Application.Interfaces;
using Diploma.Configuration;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Diploma.Application.Implementations;

public class SessionsPoolHandlerService : ISessionsPoolHandlerService
{
    private readonly IDictBaseRepository<ISessionHandlerService> _services;
    private readonly BankSettings _bankSettings;

    public SessionsPoolHandlerService(IDictBaseRepository<ISessionHandlerService> services, IConfiguration config)
    {
        _bankSettings = config.GetSection("BankSettings").Get<BankSettings>() ?? throw new ArgumentException("Не найдены настройки банка");
        _services = services;
    }
    
    public async IAsyncEnumerable<BaseResponse> AddNewBankOperationAsync(BankOperationDto operation)
    {
        ulong sessionId = operation.SessionId;
        if (_services.Contains(sessionId) == false)
        {
            _services.Add(sessionId, new SessionHandlerService(_bankSettings));
        }
        var paymentModel = new RecurringPaymentModel();
        paymentModel.SetFromDtoModel((RecurringBankOperationDto)operation);
        var responses = _services.Get(sessionId).StartRecurringPayment(paymentModel);
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