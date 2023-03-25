using System.Text.Json;
using System.Text.Json.Serialization;
using Diploma.Domain.Entities;
using Diploma.Domain.Response;
using Diploma.Service.Implementations.BankOperations;
using Diploma.Service.Interfaces;

namespace Diploma.Service.Implementations;

public class SessionHandlerService : ISessionHandlerService
{
    private const string SUCCESS_BANK_RESPONSE_CODE = "00";
    private const string SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE = "96";
    private const string SESSION_ABORTED = "Session aborted";
    private const string SESSION_SUCCESSFULLY = "Session completed successfully";
    private const int INTERMEDIATE_SESSION_COST = 50;
    
    private const decimal COST_OF_ONE_KWH = 16;
    private readonly BankHttpClient _httpClient = new();
    private IBankOperationService BankOperationService { get; set; }
    
    private decimal _sumOfSessionsByBank = 0;
    private decimal _sumOfSessionsByTOUCH = 0;
    
    public SessionHandlerService()
    {
        BankOperationService = new RecurringExecution();
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
    
    
    public async IAsyncEnumerable<BaseResponse> StartRecurringPayment(BankOperation recurringBankOperation)
    {
        _sumOfSessionsByTOUCH += recurringBankOperation.Amount;
        var operationResponse = await ExecuteRecurringPaymentAsync(recurringBankOperation);

        if (IsSessionCompletedWithoutError(operationResponse) == true)
        {
            _sumOfSessionsByBank += operationResponse.Amount;
        }
        else
        {
            yield return GetSessionResponseWithError(operationResponse);
        }
        yield return operationResponse;
        if (recurringBankOperation.WillSessionContinue == false || recurringBankOperation.Amount != INTERMEDIATE_SESSION_COST)
        {
            yield return GetSessionResponse(operationResponse);
            //стучимся в АТОЛ
        }
    }

    private async Task<RecurOperationResponse> ExecuteRecurringPaymentAsync(BankOperation recurringBankOperation)
    {
        var sendingModel = BankOperationService.GetRequestingModel(recurringBankOperation);
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