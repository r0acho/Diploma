using System.Text.Json;
using System.Text.Json.Serialization;
using Diploma.Application.Implementations.BankOperations;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations;

public class PaymentService : IPaymentService
{
    private const int COST_OF_ONE_KWH = 16;
    private readonly BankSettings _bankSettings;
    private readonly BankHttpClient _httpClient;

    private IBankOperationService _bankOperationService;

    public PaymentService(IOptions<BankSettings> bankSettings)
    {
        _bankSettings = bankSettings.Value;
        _httpClient = new BankHttpClient(bankSettings.Value.BankUrl);
    }

    public async Task<RecurOperationResponse> ExecuteRecurringPayment(RecurringPaymentModel recurringBankOperation)
    {
        _bankOperationService = new RecurringExecution(recurringBankOperation, _bankSettings);
        var sendingModel = _bankOperationService.GetRequestingModel();
        var responseMessage = await _httpClient.SendModelToBankAsync(sendingModel);
        var responseJson = await responseMessage.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };
        try
        {
            var responseBody = JsonSerializer.Deserialize<RecurOperationResponse>(responseJson, options);
            responseBody!.StatusCode = responseMessage.StatusCode;
            return responseBody;
        }
        catch (JsonException)
        {
            Console.WriteLine();
        }

        return new RecurOperationResponse();
    }
}