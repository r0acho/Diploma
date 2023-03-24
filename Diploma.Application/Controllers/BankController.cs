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
    public async IAsyncEnumerable<string> RecurPay()
    {
        RecurringBankOperation receivedData = JsonSerializer.Deserialize<RecurringBankOperation>(await Request.ReadFromJsonAsync<string>());
        var responses = _sessionsPoolHandlerService.AddNewBankOperationAsync(receivedData);
        await foreach (var response in responses)
        {
            if (response is SessionResponse sessionResponse)
            {
                yield return JsonSerializer.Serialize(sessionResponse);
            }

            if (response is RecurOperationResponse recurOperationResponse)
            {
                yield return JsonSerializer.Serialize(recurOperationResponse);
            }
            yield return JsonSerializer.Serialize(response);
        }
    }
}