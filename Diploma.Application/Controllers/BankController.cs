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
        RecurringBankOperation receivedData = JsonSerializer.Deserialize<RecurringBankOperation>(await Request.ReadFromJsonAsync<string>());
        var responses = _sessionsPoolHandlerService.AddNewBankOperationAsync(receivedData);
        await foreach (var response in responses)
        {
            yield return response;
        }
    }
}