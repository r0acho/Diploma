using Diploma.Application.Interfaces;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;


namespace Diploma.Application.Implementations;

public class SessionsPoolHandlerService : ISessionsPoolHandlerService
{
    private readonly ISessionHandlerService _serviceHandlerService;
    
    public SessionsPoolHandlerService(ISessionStatesRepository sessionStates,
        ISessionHandlerService sessionHandlerService)
    {
        _serviceHandlerService = sessionHandlerService;
    }
    
    public async IAsyncEnumerable<BaseResponse> AddNewBankOperationAsync(BankOperationDto operation)
    {
        var paymentModel = new RecurringPaymentModel();
        paymentModel.SetFromDtoModel((RecurringBankOperationDto)operation);
        var responses = _serviceHandlerService.StartRecurringPayment(operation.SessionId, paymentModel);
        await foreach (var recurringOperationResponse in responses)
        {
            yield return recurringOperationResponse;
        }
    }
}