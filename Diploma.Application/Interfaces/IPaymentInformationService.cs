using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface IPaymentInformationService
{
    Task<IEnumerable<RecurOperationResponse>> GetRecurPaymentResponses();
    Task<RecurOperationResponse> GetRecurPaymentResponseById(ulong id);
}