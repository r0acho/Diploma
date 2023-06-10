using Diploma.Domain.Entities;

namespace Diploma.Infrastructure.Interfaces;

/// <summary>
/// Определяет интерфейс для репозитория, управляющего состояниями сессии.
/// </summary>
/// <typeparam name="SessionStateModel">Тип состояния сессии, управляемый репозиторием</typeparam>
public interface ISessionStatesRepository : IBaseRepository<SessionStateModel>
{
    /// <summary>
    /// Определяет, существует ли состояние сессии с заданным идентификатором в репозитории.
    /// </summary>
    /// <param name="id">Идентификатор состояния сессии для проверки наличия в репозитории</param>
    /// <returns>Объект типа Task, содержащий булево значение, указывающее наличие или отсутствие состояния сессии в репозитории. </returns>
    Task<bool> Exists(ulong id);
}