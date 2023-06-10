namespace Diploma.Infrastructure.Interfaces;

/// <summary>
/// Интерфейс базового репозитория.
/// </summary>
/// <typeparam name="T">Тип объекта, которым оперирует репозиторий.</typeparam>
public interface IBaseRepository<T>
{
    /// <summary>
    /// Создает новый объект в репозитории.
    /// </summary>
    /// <param name="entity">Объект, который нужно создать.</param>
    Task Create(T entity);

    /// <summary>
    /// Получает объект из репозитория по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор объекта.</param>
    /// <returns>Объект с заданным идентификатором.</returns>
    Task<T> GetById(ulong id);

    /// <summary>
    /// Получает все объекты из репозитория.
    /// </summary>
    /// <returns>Коллекцию всех объектов.</returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Удаляет объект из репозитория по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор объекта, который нужно удалить.</param>
    Task DeleteById(ulong id);

    /// <summary>
    /// Обновляет существующий объект в репозитории.
    /// </summary>
    /// <param name="entity">Объект, который нужно обновить.</param>
    Task Update(T entity);
}