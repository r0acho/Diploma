using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Diploma.Domain.Dto;
using Diploma.Domain.Responses;
using Diploma.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class BankController : ControllerBase
{
    private IConfiguration _config;
    private readonly ISessionsPoolHandlerService _sessionsPoolHandlerService;
    private readonly JsonSerializerOptions _options = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
        WriteIndented = true
    };

    public BankController(ISessionsPoolHandlerService sessionsPoolHandlerService, IConfiguration config)
    {
        _config = config;
        _sessionsPoolHandlerService = sessionsPoolHandlerService;
    }

    [HttpPost(Name = "StartRecurPayment")]
    public async IAsyncEnumerable<string> RecurPay([FromBody] RecurringBankOperationDto receivedData)
    {
        if (!ModelState.IsValid) yield break;
        var responses = _sessionsPoolHandlerService.AddNewBankOperationAsync(receivedData);
        await foreach (var response in responses)
        {
            if (response is SessionResponse sessionResponse)
            {
                yield return JsonSerializer.Serialize(sessionResponse, _options);
            }
            else if (response is RecurOperationResponse recurOperationResponse)
            {
                yield return JsonSerializer.Serialize(recurOperationResponse, _options);
            }
            else if (response is FiscalPaymentResponse fiscalOperationResponse)
            {
                yield return JsonSerializer.Serialize(fiscalOperationResponse, _options);
            }
            else yield return JsonSerializer.Serialize(response, _options);
        }
    }
}