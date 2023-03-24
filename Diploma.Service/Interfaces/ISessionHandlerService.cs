using Diploma.Domain.Entities;
using Diploma.Domain.Response;

namespace Diploma.Service.Interfaces;

public interface ISessionHandlerService
{
    public IAsyncEnumerable<BaseResponse> StartRecurringPayment(BankOperation recurringBankOperation);
}