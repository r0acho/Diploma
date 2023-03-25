using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
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
    private readonly JsonSerializerOptions _options = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
        WriteIndented = true
    };

    public BankController(ISessionsPoolHandlerService sessionsPoolHandlerService)
    {
        _sessionsPoolHandlerService = sessionsPoolHandlerService;
    }

    [HttpPost(Name = "StartRecurPayment")]
    public async IAsyncEnumerable<string> RecurPay()
    {
        List<string> responsesStrings = new();
        RecurringBankOperation? receivedData = null;
        try
        {
             receivedData =
                JsonSerializer.Deserialize<RecurringBankOperation>(await Request.ReadFromJsonAsync<string>()!, _options);
        }
        catch (JsonException jsonException)
        {
            BaseResponse baseResponse = new()
            {
                Description = jsonException.Message,
                StatusCode = HttpStatusCode.BadRequest
            };
            responsesStrings.Add(JsonSerializer.Serialize(baseResponse, _options));
        }

        if (receivedData != null)
        {
            var responses = _sessionsPoolHandlerService.AddNewBankOperationAsync(receivedData);
            await foreach (var response in responses)
            {
                if (response is SessionResponse sessionResponse)
                {
                    responsesStrings.Add(JsonSerializer.Serialize(sessionResponse, _options));
                }

                else if (response is RecurOperationResponse recurOperationResponse)
                {
                    responsesStrings.Add(JsonSerializer.Serialize(recurOperationResponse, _options));
                }
                else responsesStrings.Add(JsonSerializer.Serialize(response, _options));
            }
        }

        foreach (var response in responsesStrings)
        {
            yield return response;
        }

    }
}