using Diploma.Application.Interfaces;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Application.Implementations;

/// <summary>
/// Реализация сервиса для получения информации о рекуррентных платежах.
/// </summary>
public class PaymentInformationService : IPaymentInformationService
{
    private readonly IResponsesRepository<RecurOperationResponse> _recurResponsesRepository;

    public PaymentInformationService(IResponsesRepository<RecurOperationResponse> recurResponsesRepository)
    {
        _recurResponsesRepository = recurResponsesRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RecurOperationResponse>> GetRecurPaymentResponses()
    {
        return await _recurResponsesRepository.GetAll();
    }

    /// <inheritdoc/>
    public async Task<RecurOperationResponse> GetRecurPaymentResponseById(ulong id)
    {
        return await _recurResponsesRepository.GetById(id);
    }
}