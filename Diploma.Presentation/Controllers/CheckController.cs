using Diploma.Application.Interfaces;
using Diploma.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckController
{
    private readonly ICheckInformationService _checkInformationService;

    public CheckController(ICheckInformationService checkInformationService)
    {
        _checkInformationService = checkInformationService;
    }
    
    [HttpGet]
    [Route("GetChecks/")]
    public async Task<IEnumerable<FiscalizeResponse>> GetAllChecks()
    {
        return await _checkInformationService.GetChecks();
    }

    [HttpGet]
    [Route("GetChecks/{id}")]
    public async Task<FiscalizeResponse> GetCheckById(ulong id)
    {
        return await _checkInformationService.GetCheckById(id);
    }
    
    [HttpGet]
    [Route("GetChecks/{uuid}")]
    public async Task<FiscalizeResponse> GetCheckByUuid(string uuid)
    {
        return await _checkInformationService.GetCheckByUuId(uuid);
    }
}