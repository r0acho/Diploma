using Diploma.Domain.Responses;

namespace Diploma.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс репозитория ответов.
/// </summary>
/// <typeparam name="T">Тип объекта ответа.</typeparam>
public interface IResponsesRepository<T> where T : BaseResponse
{
    /// <summary>
    /// Создает новый ответ в репозитории.
    /// </summary>
    /// <param name="entity">Объект ответа, который нужно создать.</param>
    Task Create(T entity);

    /// <summary>
    /// Получает все ответы из репозитория.
    /// </summary>
    /// <returns>Коллекцию всех ответов.</returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Получает ответ из репозитория по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор ответа.</param>
    /// <returns>Объект ответа с заданным идентификатором.</returns>
    Task<T> GetById(ulong id);
}