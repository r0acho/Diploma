using Diploma.Application.Interfaces;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Application.Implementations;

/// <summary>
/// Реализация сервиса для работы с информацией о состоянии сессий.
/// </summary>
public class SessionInformationService : ISessionInformationService
{
    /// <summary>
    /// Репозиторий, предоставляющий доступ к состояниям сессий.
    /// </summary>
    private readonly ISessionStatesRepository _sessionStatesRepository;

    public SessionInformationService(ISessionStatesRepository sessionStatesRepository)
    {
        _sessionStatesRepository = sessionStatesRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<SessionStateModel>> GetSessionStates()
    {
        return await _sessionStatesRepository.GetAll();
    }

    /// <inheritdoc/>
    public async Task<SessionStateModel> GetSessionStateById(ulong id)
    {
        return await _sessionStatesRepository.GetById(id);
    }

    /// <inheritdoc/>
    public async Task DeleteSessionById(ulong id)
    {
        await _sessionStatesRepository.DeleteById(id);
    }

    /// <inheritdoc/>
    public async Task UpdateSession(SessionStateModel model)
    {
        await _sessionStatesRepository.Update(model);
    }
}