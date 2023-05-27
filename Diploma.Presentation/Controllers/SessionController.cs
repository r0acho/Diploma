using Diploma.Application.Interfaces;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionController : ControllerBase
{
    private readonly ISessionInformationService _sessionInformationService;

    public SessionController(ISessionInformationService sessionInformationService)
    {
        _sessionInformationService = sessionInformationService;
    }

    [HttpGet("GetSessionStates")]
    public async Task<IEnumerable<SessionStateModel>> GetSessionStates()
    {
        return await _sessionInformationService.GetSessionStates();
    }
    

    [HttpGet("GetSessionStateById")]
    public async Task<SessionStateModel> GetSessionStateById(ulong id)
    {
        return await _sessionInformationService.GetSessionStateById(id);
    }
}