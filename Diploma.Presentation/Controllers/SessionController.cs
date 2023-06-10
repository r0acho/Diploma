using Diploma.Application.Interfaces;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

/// <summary>
/// API для получения информации о сессиях из БД
/// </summary>
[ApiController]
[Route("[controller]")]
public class SessionController : ControllerBase
{
    private readonly ISessionInformationService _sessionInformationService;

    public SessionController(ISessionInformationService sessionInformationService)
    {
        _sessionInformationService = sessionInformationService;
    }

    /// <summary>
    /// Получить информацию о всех сессиях из БД
    /// </summary>
    /// <returns>Список сессий</returns>
    [HttpGet("GetSessions")]
    public async Task<IEnumerable<SessionStateModel>> GetSessionStates()
    {
        return await _sessionInformationService.GetSessionStates();
    }
    
    /// <summary>
    /// Получить информацию по сессии по ее ID
    /// </summary>
    /// <param name="id">ID сессии</param>
    /// <returns>Состояние сессии</returns>
    [HttpGet("GetSessions/{id}")]
    public async Task<SessionStateModel> GetSessionStateById(ulong id)
    {
        return await _sessionInformationService.GetSessionStateById(id);
    }

    /// <summary>
    /// Удалить сессию по ее ID
    /// </summary>
    /// <param name="id">ID сессии</param>
    /// <returns>Status-code результата операции</returns>
    [HttpDelete("GetSessions/{id}")]
    public async Task<IActionResult> DeleteSessionById(ulong id)
    {
        await _sessionInformationService.DeleteSessionById(id);
        return Ok(200);
    }
}