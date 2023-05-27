using Diploma.Application.Interfaces;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Application.Implementations;

public class PaymentInformationService : IPaymentInformationService
{
    private readonly IResponsesRepository<RecurOperationResponse> _recurResponsesRepository;

    public PaymentInformationService(IResponsesRepository<RecurOperationResponse> recurResponsesRepository)
    {
        _recurResponsesRepository = recurResponsesRepository;
    }

    public async Task<IEnumerable<RecurOperationResponse>> GetRecurPaymentResponses()
    {
        return await _recurResponsesRepository.GetAll();
    }

    public async Task<RecurOperationResponse> GetRecurPaymentResponseById(ulong id)
    {
        return await _recurResponsesRepository.GetById(id);
    }
}