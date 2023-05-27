using Diploma.Domain.Entities;

namespace Diploma.Application.Interfaces;

public interface ISessionHandlerService
{
    public Task StartRecurringPayment(ulong idSession, RecurringPaymentModel recurringBankOperation);
}