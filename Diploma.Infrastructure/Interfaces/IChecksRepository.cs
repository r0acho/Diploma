using Diploma.Domain.Responses;

namespace Diploma.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс репозитория чеков.
/// </summary>
public interface IChecksRepository
{
    /// <summary>
    /// Создает новый чек в репозитории.
    /// </summary>
    /// <param name="entity">Объект чека, который нужно создать.</param>
    Task Create(FiscalizeResponse entity);

    /// <summary>
    /// Получает все чеки из репозитория.
    /// </summary>
    /// <returns>Коллекцию всех чеков.</returns>
    Task<IEnumerable<FiscalizeResponse>> GetAll();

    /// <summary>
    /// Получает чек из репозитория по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор чека.</param>
    /// <returns>Объект чека с заданным идентификатором.</returns>
    Task<FiscalizeResponse> GetById(ulong id);
}