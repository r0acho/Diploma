using Diploma.Domain.Entities;
using Diploma.Domain.Response;

namespace Diploma.Application.Interfaces;

public interface ISessionHandlerService
{
    public IAsyncEnumerable<BaseResponse> StartRecurringPayment(BankOperation recurringBankOperation);
}