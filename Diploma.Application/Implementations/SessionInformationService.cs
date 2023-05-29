using Diploma.Application.Interfaces;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Application.Implementations;

public class SessionInformationService : ISessionInformationService
{
    private readonly ISessionStatesRepository _sessionStatesRepository;

    public SessionInformationService(ISessionStatesRepository sessionStatesRepository)
    {
        _sessionStatesRepository = sessionStatesRepository;
    }

    public async Task<IEnumerable<SessionStateModel>> GetSessionStates()
    {
        return await _sessionStatesRepository.GetAll();
    }

    public async Task<SessionStateModel> GetSessionStateById(ulong id)
    {
        return await _sessionStatesRepository.GetById(id);
    }

    public async Task DeleteSessionById(ulong id)
    {
        await _sessionStatesRepository.DeleteById(id);
    }

    public async Task UpdateSession(SessionStateModel model)
    {
        await _sessionStatesRepository.Update(model);
    }
}