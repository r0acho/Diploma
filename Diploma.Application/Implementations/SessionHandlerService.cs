using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diploma.Application.Implementations;

public class SessionHandlerService : ISessionHandlerService
{
    private const string SUCCESS_BANK_RESPONSE_CODE = "00";
    private const string SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE = "96";
    private const string SESSION_ABORTED = "Session aborted";
    private const string SESSION_SUCCESSFULLY = "Session completed successfully";
    private const int INTERMEDIATE_SESSION_COST = 50;
    private const int COST_OF_ONE_KWH = 16;
    
    private readonly BankSettings _bankSettings;
    private readonly ISessionStatesRepository _sessionStates;
    private readonly ILogger<ISessionHandlerService> _logger;
    
    private readonly IFiscalizePaymentService _fiscalizePaymentService;
    private readonly IPaymentService _paymentService;
    private SessionStateModel? _currentSessionStateModel;
    
    
    public SessionHandlerService(IOptions<BankSettings> bankSettings, ILogger<ISessionHandlerService> logger, 
        IPaymentService paymentService, IFiscalizePaymentService fiscalizePaymentService, 
        ISessionStatesRepository sessionStatesRepository)
    {
        _logger = logger;
        _sessionStates = sessionStatesRepository;
        _fiscalizePaymentService = fiscalizePaymentService;
        _bankSettings = bankSettings.Value;
        _paymentService = paymentService;
    }
    
    private static bool IsPaymentCompletedWithoutError(RecurOperationResponse response)
    {
        return response.ResponseCode is SUCCESS_BANK_RESPONSE_CODE or SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE;
    }

    private SessionResponse GetSessionResponse(RecurOperationResponse response)
    {
        return new SessionResponse
        {
            Description = IsPaymentCompletedWithoutError(response) ? SESSION_SUCCESSFULLY : SESSION_ABORTED,
            BankAmount = _currentSessionStateModel!.SumOfSessionsByBank,
            TouchAmount = _currentSessionStateModel!.SumOfSessionsByTouch,
            CardNumber = response.CardNumber,
            ResponseText = response.ResponseText,
            Id = _currentSessionStateModel!.Id,
            SessionStatus = _currentSessionStateModel!.Status
        };
    }
    
    private static void AddItemToReceipt(PaymentModel paymentModel, ICollection<ItemFiscalReceiptDto> items)
    {
        items.Add(new ItemFiscalReceiptDto
        {
            Id = paymentModel.Order,
            Description = paymentModel.Description,
            Price = (uint)paymentModel.Amount * 100,
            QtyDecimal = paymentModel.Amount / COST_OF_ONE_KWH,
            TaxId = 1, //узнать процент налога
            PayAttribute = 4 //какой нужен?
        });
    }
    

    private async Task<FiscalPaymentResponse> Fiscalize(RecurringPaymentModel paymentModel, 
        decimal sum, IEnumerable<ItemFiscalReceiptDto> items)
    {
        var fiscalReceiptDto = new FiscalReceiptDto
        {
            PhoneOrEmail = paymentModel.Email!, TaxMode = 1,
            Items = items, NonCash = new [] { (uint)sum * 100 },
            Place = paymentModel.ModuleUrl!
        };

        return await _fiscalizePaymentService.FiscalizePayment(fiscalReceiptDto, paymentModel);
    }
    
    private void SetSessionStatusAfterBankOperation(RecurOperationResponse recurOperationResponse,
        RecurringPaymentModel recurringPayment)
    {
        if (IsPaymentCompletedWithoutError(recurOperationResponse) == false)
        {
            _logger.LogWarning($"Недостаточно средств для оплаты сессии {_currentSessionStateModel!.Id}");
            _currentSessionStateModel.Status = SessionStatus.InsufficientFundsError;
        }
        else
        {
            _logger.LogInformation($"Успешно выполнен промежуточный платеж в сессии {_currentSessionStateModel!.Id}");
            _currentSessionStateModel.SumOfSessionsByBank += recurOperationResponse.Amount;
            AddItemToReceipt(recurringPayment, _currentSessionStateModel.Items);
        }
    }

    private void SetSessionStatusAfterEndOfSession(FiscalPaymentResponse fiscalPaymentResponse)
    {
        if (_currentSessionStateModel.SumOfSessionsByBank != _currentSessionStateModel.SumOfSessionsByTouch)
        {
            _logger.LogWarning($"Ошибка сверки в сессии {_currentSessionStateModel!.Id}");
            _currentSessionStateModel.Status = SessionStatus.ReconciliationError;
        }

        //TODO добавить проверку ошибки фискализации чека

        if (_currentSessionStateModel.Status != SessionStatus.InProgress) return;
        _logger.LogInformation($"Сессия {_currentSessionStateModel!.Id} успешно завершена");
        _currentSessionStateModel.Status = SessionStatus.Success;

    }
    
    private void SetSessionStatusAfterEndOfSession()
    {
        if (_currentSessionStateModel.SumOfSessionsByBank != _currentSessionStateModel.SumOfSessionsByTouch)
        {
            _logger.LogWarning($"Ошибка сверки в сессии {_currentSessionStateModel.Id}");
            _currentSessionStateModel.Status = SessionStatus.ReconciliationError;
        }

        //TODO добавить проверку ошибки фискализации чека

        if (_currentSessionStateModel.Status != SessionStatus.InProgress) return;
        _logger.LogInformation($"Сессия {_currentSessionStateModel.Id} успешно завершена");
        _currentSessionStateModel.Status = SessionStatus.Success;

    }
    
    public async IAsyncEnumerable<BaseResponse> StartRecurringPayment(ulong sessionId,
        RecurringPaymentModel recurringPayment)
    {
        _currentSessionStateModel = await GetOrCreateSession(sessionId);
        if (_currentSessionStateModel.Status == SessionStatus.Success)
        {
            yield return new BaseResponse();
        }
        _currentSessionStateModel.SumOfSessionsByTouch += recurringPayment.Amount;
        var operationResponse = await _paymentService.ExecuteRecurringPayment(recurringPayment);
        SetSessionStatusAfterBankOperation(operationResponse, recurringPayment);
        yield return operationResponse;
        if (_currentSessionStateModel.Status == SessionStatus.InsufficientFundsError ||
            operationResponse.Amount != INTERMEDIATE_SESSION_COST) //заканчиваем сессию по причине нехватки средств и/или последней операции в сессии
        {
            //var fiscalResponse = await Fiscalize(recurringPayment, _currentSessionStateModel.SumOfSessionsByBank, _currentSessionStateModel.Items);
            //SetSessionStatusAfterEndOfSession(fiscalResponse);
            SetSessionStatusAfterEndOfSession();
            yield return GetSessionResponse(operationResponse);
            //yield return fiscalResponse;
        }
        await _sessionStates.Update(_currentSessionStateModel);
    }

    private async Task<SessionStateModel> GetOrCreateSession(ulong sessionId)
    {
        if (await _sessionStates.Exists(sessionId) == false)
        {
            await _sessionStates.Create(new SessionStateModel { Id = sessionId });
        }

        return await _sessionStates.GetById(sessionId);
    }
}