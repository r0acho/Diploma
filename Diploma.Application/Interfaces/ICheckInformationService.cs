using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с чеками.
/// </summary>
public interface ICheckInformationService
{
    /// <summary>
    /// Получение списка чеков.
    /// </summary>
    /// <returns>Список чеков.</returns>
    Task<IEnumerable<FiscalizeResponse>> GetChecks();

    /// <summary>
    /// Получение чека по его Id.
    /// </summary>
    /// <param name="id">Id чека.</param>
    /// <returns>Чек.</returns>
    Task<FiscalizeResponse> GetCheckById(ulong id);

    /// <summary>
    /// Получение чека по его UUID.
    /// </summary>
    /// <param name="uuid">UUID чека.</param>
    /// <returns>Чек.</returns>
    Task<FiscalizeResponse> GetCheckByUuId(string uuid);
}