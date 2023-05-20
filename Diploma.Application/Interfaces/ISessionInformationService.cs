using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface ISessionInformationService
{
    Task<IEnumerable<SessionStateModel>> GetSessionStates();
    Task<SessionStateModel> GetSessionStateById(ulong id);
    Task<IEnumerable<SessionResponse>> GetSessionResponses();
    Task<SessionResponse> GetSessionResponseById(ulong id);
}