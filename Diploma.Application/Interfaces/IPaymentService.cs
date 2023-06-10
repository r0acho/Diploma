using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для проведения платежей.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Выполнение регулярного платежа.
    /// </summary>
    /// <param name="recurringBankOperation">Модель регулярных платежей.</param>
    /// <returns>Ответ от банка на выполненный платеж.</returns>
    Task<RecurOperationResponse> ExecuteRecurringPayment(RecurringPaymentModel recurringBankOperation);
}