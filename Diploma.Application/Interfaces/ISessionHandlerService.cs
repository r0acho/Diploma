using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface ISessionHandlerService
{
    public Task StartRecurringPayment(ulong idSession,RecurringPaymentModel recurringBankOperation);
}