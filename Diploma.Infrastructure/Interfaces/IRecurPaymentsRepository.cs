using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Infrastructure.Interfaces;

public interface IRecurPaymentsRepository : IBaseRepository<RecurringPaymentModel>
{
    PaymentStatus GetStatus(ulong id);
}