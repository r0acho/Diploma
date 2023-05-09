using System.Text.Json;
using System.Text.Json.Serialization;
using Diploma.Application.Implementations.BankOperations;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Implementations;

public class PaymentService : IPaymentService
{
    private readonly BankSettings _bankSettings;
    private readonly BankHttpClient _httpClient;
    private IBankOperationService? _bankOperationService;
    
    private const int COST_OF_ONE_KWH = 16;


    public PaymentService(BankSettings bankSettings)
    {
        _bankSettings = bankSettings;
        _httpClient = new BankHttpClient(bankSettings.BankUrl);
    }

    public PaymentService(BankSettings bankSettings, IBankOperationService service) : this(bankSettings)
    {
        _bankOperationService = service;
    }
    
    
    public async Task<RecurOperationResponse> ExecuteRecurringPayment(RecurringPaymentModel recurringBankOperation)
    {
        if (_bankOperationService == null) _bankOperationService = new RecurringExecution(recurringBankOperation, _bankSettings);
        var sendingModel = _bankOperationService.GetRequestingModel();
        var responseMessage = await _httpClient.SendModelToBankAsync(sendingModel);
        string responseJson = await responseMessage.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString 
        };
        var responseBody = JsonSerializer.Deserialize<RecurOperationResponse>(responseJson, options);
        responseBody!.StatusCode = responseMessage.StatusCode;
        return responseBody;
    }
}