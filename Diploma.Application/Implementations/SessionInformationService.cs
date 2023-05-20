using Diploma.Application.Interfaces;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Application.Implementations;

public class SessionInformationService : ISessionInformationService
{
    private readonly ISessionStatesRepository _sessionStatesRepository;
    private readonly IResponsesRepository<SessionResponse> _responsesRepository;

    public SessionInformationService(ISessionStatesRepository sessionStatesRepository, 
        IResponsesRepository<SessionResponse> responsesRepository)
    {
        _sessionStatesRepository = sessionStatesRepository;
        _responsesRepository = responsesRepository;
    }
    
    public async Task<IEnumerable<SessionStateModel>> GetSessionStates()
    {
        return await _sessionStatesRepository.GetAll();
    }

    public async Task<SessionStateModel> GetSessionStateById(ulong id)
    {
        return await _sessionStatesRepository.GetById(id);
    }

    public async Task<IEnumerable<SessionResponse>> GetSessionResponses()
    {
        return await _responsesRepository.GetAll();
    }

    public async Task<SessionResponse> GetSessionResponseById(ulong id)
    {
        return await _responsesRepository.GetById(id);
    }
}