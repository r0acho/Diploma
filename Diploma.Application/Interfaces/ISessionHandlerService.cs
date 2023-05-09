using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface ISessionHandlerService
{
    public IAsyncEnumerable<BaseResponse> StartRecurringPayment(ulong idSession,RecurringPaymentModel recurringBankOperation);
}