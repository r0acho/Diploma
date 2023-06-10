using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с информацией о состоянии сессий.
/// </summary>
public interface ISessionInformationService
{
    /// <summary>
    /// Получение списка состояний сессий.
    /// </summary>
    /// <returns>Список состояний сессий.</returns>
    Task<IEnumerable<SessionStateModel>> GetSessionStates();

    /// <summary>
    /// Получение состояния сессии по ее Id.
    /// </summary>
    /// <param name="id">Id сессии.</param>
    /// <returns>Состояние сессии.</returns>
    Task<SessionStateModel> GetSessionStateById(ulong id);

    /// <summary>
    /// Удаление информации о сессии по ее Id.
    /// </summary>
    /// <param name="id">Id сессии.</param>
    /// <returns>Код ответа удаления.</returns>
    Task DeleteSessionById(ulong id);

    /// <summary>
    /// Обновление информации о сессии.
    /// </summary>
    /// <param name="model">Модель с обновленной информацией о сессии.</param>
    /// <returns>Код ответа обновления.</returns>
    Task UpdateSession(SessionStateModel model);
}