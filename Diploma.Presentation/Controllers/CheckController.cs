using Diploma.Application.Interfaces;
using Diploma.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

/// <summary>
/// API для получения информации о фискальных чеках из БД
/// </summary>
[ApiController]
[Route("[controller]")]
public class CheckController
{
    private readonly ICheckInformationService _checkInformationService;

    public CheckController(ICheckInformationService checkInformationService)
    {
        _checkInformationService = checkInformationService;
    }
    
    /// <summary>
    /// Получить информацию о всех чеках
    /// </summary>
    /// <returns>Список фискальных чеков</returns>
    [HttpGet]
    [Route("GetChecks/")]
    public async Task<IEnumerable<FiscalizeResponse>> GetAllChecks()
    {
        return await _checkInformationService.GetChecks();
    }

    /// <summary>
    /// Получить информацию по чеку по переданному ID
    /// </summary>
    /// <param name="id">ID чека</param>
    /// <returns>Фискальный чек по его ID</returns>
    [HttpGet]
    [Route("GetChecks/{id}")]
    public async Task<FiscalizeResponse> GetCheckById(ulong id)
    {
        return await _checkInformationService.GetCheckById(id);
    }
    
    /// <summary>
    /// Получить информацию по чеку по переданному UUID
    /// </summary>
    /// <param name="uuid">UUID чека</param>
    /// <returns>Фискальный чек по его UUID</returns>
    [HttpGet]
    [Route("GetChecks/{uuid}")]
    public async Task<FiscalizeResponse> GetCheckByUuid(string uuid)
    {
        return await _checkInformationService.GetCheckByUuId(uuid);
    }
}