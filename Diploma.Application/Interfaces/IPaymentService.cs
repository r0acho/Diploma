using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface IPaymentService
{
    Task<RecurOperationResponse> ExecuteRecurringPayment(RecurringPaymentModel recurringBankOperation);
}