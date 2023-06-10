using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с информацией о регулярных платежах.
/// </summary>
public interface IPaymentInformationService
{
    /// <summary>
    /// Получение списка ответов на регулярные платежи.
    /// </summary>
    /// <returns>Список ответов на регулярные платежи.</returns>
    Task<IEnumerable<RecurOperationResponse>> GetRecurPaymentResponses();

    /// <summary>
    /// Получение ответа на регулярный платеж по его Id.
    /// </summary>
    /// <param name="id">Id ответа на регулярный платеж.</param>
    /// <returns>Ответ на регулярный платеж.</returns>
    Task<RecurOperationResponse> GetRecurPaymentResponseById(ulong id);
}