using Diploma.Domain.Entities;

namespace Diploma.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для обработки сессий.
/// </summary>
public interface ISessionHandlerService
{
    /// <summary>
    /// Запуск регулярного платежа для сессии.
    /// </summary>
    /// <param name="idSession">Id сессии.</param>
    /// <param name="recurringBankOperation">Модель регулярных платежей.</param>
    /// <returns>Код ответа запуска регулярного платежа.</returns>
    public Task StartRecurringPayment(ulong idSession, RecurringPaymentModel recurringBankOperation);
}