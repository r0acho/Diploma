using System.Text.Json;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class BankController : ControllerBase
{
    private readonly ISessionsPoolHandlerService _sessionsPoolHandlerService;

    public BankController(ISessionsPoolHandlerService sessionsPoolHandlerService)
    {
        _sessionsPoolHandlerService = sessionsPoolHandlerService;
    }

    [HttpPost(Name = "StartRecurPayment")]
    public async IAsyncEnumerable<BaseResponse> RecurPay()
    {
        var receivedData = JsonSerializer.Deserialize<RecurringBankOperation>(await Request.ReadFromJsonAsync<string>());
        _sessionsPoolHandlerService.AddNewBankOperationAsync(receivedData);
        var responses = _sessionsPoolHandlerService.GetOperationResponsesAsync(receivedData.SessionId);
        await foreach (var response in responses)
        {
            yield return response;
        }
    }
}