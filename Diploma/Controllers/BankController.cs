using System.Dynamic;
using Diploma.Domain.Entities;
using Diploma.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Controllers;

public class BankController : Controller
{
    private readonly ISessionHandlerService _sessionHandlerService;

    public BankController(ISessionHandlerService sessionHandlerService)
    {
        this._sessionHandlerService = sessionHandlerService;
    }

    private static Dictionary<string, string> GetReceivedModel(IDictionary<string, object?> receivedModel)
    {
        var newModel = new Dictionary<string, string>();

        foreach (var pair in receivedModel)
        {
            newModel[pair.Key] = pair.Value.ToString();
        }
        return newModel;
    }
    
    [HttpPost]
    public async Task<string> Pay()
    {
        var receivedData = new BankOperation(GetReceivedModel(await Request.ReadFromJsonAsync<ExpandoObject>()));
        return await _sessionHandlerService.GetBankResponse(receivedData);
    }
}