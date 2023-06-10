using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для фискализации платежей.
/// </summary>
public interface IFiscalizePaymentService
{
    /// <summary>
    /// Фискализация платежа.
    /// </summary>
    /// <param name="sessionStateModel">Модель состояния сессии.</param>
    /// <param name="lastPaymentModel">Модель последнего операционного платежа.</param>
    /// <param name="costOfOneKwh">Стоимость одного КВт*ч.</param>
    /// <returns>Ответ от сервиса фискализации на выполненный платеж.</returns>
    Task<FiscalizeResponse> FiscalizePayment(SessionStateModel sessionStateModel, RecurringPaymentModel lastPaymentModel, decimal costOfOneKwh);
}