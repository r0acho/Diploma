using System.Text.Json;
using System.Text.Json.Serialization;
using Diploma.Application.Implementations.BankOperations;
using Diploma.Application.Interfaces;
using Diploma.Configuration;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Microsoft.Extensions.Configuration;

namespace Diploma.Application.Implementations;

public class SessionHandlerService : ISessionHandlerService
{
    private const string SUCCESS_BANK_RESPONSE_CODE = "00";
    private const string SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE = "96";
    private const string SESSION_ABORTED = "Session aborted";
    private const string SESSION_SUCCESSFULLY = "Session completed successfully";
    private const int INTERMEDIATE_SESSION_COST = 50;
    
    private const decimal COST_OF_ONE_KWH = 16;
    private readonly BankHttpClient _httpClient;
    private readonly BankSettings _bankSettings;
    private IBankOperationService? _bankOperationService { get; set; }
    
    private decimal _sumOfSessionsByBank = 0;
    private decimal _sumOfSessionsByTOUCH = 0;
    
    public SessionHandlerService(BankSettings bankSettings)
    {
        _bankSettings = bankSettings;
        _httpClient = new(bankSettings.BankUrl);
        //BankOperationService = new RecurringExecution();
    }

    public SessionHandlerService(BankSettings bankSettings, IBankOperationService service)
    {
        _bankSettings = bankSettings;
        _httpClient = new(bankSettings.BankUrl);
        _bankOperationService = service;
    }

    private bool IsSessionCompletedWithoutError(RecurOperationResponse response)
    {
        return response.ResponseCode == SUCCESS_BANK_RESPONSE_CODE || response.ResponseCode == SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE;
    }

    private SessionResponse GetSessionResponse(RecurOperationResponse response)
    {
        return new SessionResponse
        {
            Description = SESSION_SUCCESSFULLY,
            BankAmount = _sumOfSessionsByBank,
            TouchAmount = _sumOfSessionsByTOUCH,
            CardNumber = response.CardNumber,
            ResponseText = response.ResponseText
        };
    }
    
    private SessionResponse GetSessionResponseWithError(RecurOperationResponse response)
    {
        return new SessionResponse
        {
            Description = SESSION_ABORTED,
            BankAmount = _sumOfSessionsByBank,
            TouchAmount = _sumOfSessionsByTOUCH,
            CardNumber = response.CardNumber,
            ResponseText = response.ResponseText
        };
    }
    
    
    public async IAsyncEnumerable<BaseResponse> StartRecurringPayment(RecurringPaymentModel paymentModel)
    {
        _sumOfSessionsByTOUCH += paymentModel.Amount;
        var operationResponse = await ExecuteRecurringPaymentAsync(paymentModel);

        if (IsSessionCompletedWithoutError(operationResponse) == true)
        {
            _sumOfSessionsByBank += operationResponse.Amount;
        }
        else
        {
            yield return GetSessionResponseWithError(operationResponse);
        }
        yield return operationResponse;
        if (paymentModel.Amount != INTERMEDIATE_SESSION_COST)
        {
            yield return GetSessionResponse(operationResponse);
            //стучимся в АТОЛ
        }
    }

    private async Task<RecurOperationResponse> ExecuteRecurringPaymentAsync(RecurringPaymentModel recurringBankOperation)
    {
        if (_bankOperationService == null) _bankOperationService = new RecurringExecution(recurringBankOperation, _bankSettings.SecretKey);
        var sendingModel = _bankOperationService.GetRequestingModel();
        var responseMessage = await _httpClient.SendModelToBankAsync(sendingModel, _bankSettings.BankUrl!);
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