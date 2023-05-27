using Diploma.Application.Interfaces;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Diploma.Application.Implementations;

public class SessionHandlerService : ISessionHandlerService
{
    private const string SUCCESS_BANK_RESPONSE_CODE = "00";
    private const string SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE = "96";
    private const string SESSION_ABORTED = "Session aborted";
    private const string SESSION_SUCCESSFULLY = "Session completed successfully";
    private const int INTERMEDIATE_SESSION_COST = 50;
    private const int COST_OF_ONE_KWH = 16;

    private readonly IFiscalizePaymentService _fiscalizePaymentService;
    private readonly IChecksRepository _checksRepository;

    private readonly ILogger<ISessionHandlerService> _logger;
    private readonly IPaymentService _paymentService;

    private readonly IResponsesRepository<RecurOperationResponse> _recurOperationResponsesRepository;
    private readonly ISessionStatesRepository _sessionStatesRepository;
    private SessionStateModel? _currentSessionStateModel;

    public SessionHandlerService(ILogger<SessionHandlerService> logger,
        IPaymentService paymentService, IFiscalizePaymentService fiscalizePaymentService,
        IResponsesRepository<RecurOperationResponse> recurOperationResponsesRepository,
        IChecksRepository checksRepository,
        ISessionStatesRepository sessionStatesRepository)
    {
        _logger = logger;
        _fiscalizePaymentService = fiscalizePaymentService;
        _recurOperationResponsesRepository = recurOperationResponsesRepository;
        _checksRepository = checksRepository;
        _sessionStatesRepository = sessionStatesRepository;
        _paymentService = paymentService;
    }

    public async Task StartRecurringPayment(ulong sessionId,
        RecurringPaymentModel recurringPayment)
    {
        if (recurringPayment.Amount <= 0) throw new ArgumentException("Некорректная сумма платежа");
        _currentSessionStateModel = await GetOrCreateSession(sessionId);
        _currentSessionStateModel.SumOfSessionsByTouch += recurringPayment.Amount;
        var operationResponse = await _paymentService.ExecuteRecurringPayment(recurringPayment);
        SetSessionStatusAfterBankOperation(operationResponse, recurringPayment);
        await _recurOperationResponsesRepository.Create(operationResponse);
        if (_currentSessionStateModel.Status == SessionStatus.InsufficientFundsError ||
            operationResponse.Amount !=
            INTERMEDIATE_SESSION_COST) //заканчиваем сессию по причине нехватки средств и/или последней операции в сессии
        {
            var fiscalResponse = await _fiscalizePaymentService.FiscalizePayment(_currentSessionStateModel, recurringPayment, COST_OF_ONE_KWH);
            SetSessionStatusAfterEndOfSession(fiscalResponse);
            await _checksRepository.Create(fiscalResponse);
        }

        await _sessionStatesRepository.Update(_currentSessionStateModel);
    }

    private static bool IsPaymentCompletedWithoutError(RecurOperationResponse response)
    {
        return response.ResponseCode is SUCCESS_BANK_RESPONSE_CODE or SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE;
    }
    
    private static void AddItemToReceipt(PaymentModel paymentModel, ICollection<ItemFiscalReceiptDto> items)
    {
        items.Add(new ItemFiscalReceiptDto
        {
            Id = paymentModel.Order,
            Description = paymentModel.Description,
            Price = (uint)paymentModel.Amount * 100,
            QtyDecimal = paymentModel.Amount / COST_OF_ONE_KWH,
        });
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

    private void SetSessionStatusAfterEndOfSession(FiscalizeResponse fiscalPaymentResponse)
    {
        if (_currentSessionStateModel.SumOfSessionsByBank != _currentSessionStateModel.SumOfSessionsByTouch)
        {
            _logger.LogWarning($"Ошибка сверки в сессии {_currentSessionStateModel!.Id}");
            _currentSessionStateModel.Status = SessionStatus.ReconciliationError;
        }

        if (fiscalPaymentResponse.ErrorJson != null)
        {
            _logger.LogWarning($"Ошибка фискализации в сессии {_currentSessionStateModel!.Id}");
            _currentSessionStateModel.Status = SessionStatus.FiscalizationError;
        }

        if (_currentSessionStateModel.Status != SessionStatus.InProgress) return;
        _logger.LogInformation($"Сессия {_currentSessionStateModel!.Id} успешно завершена");
        _currentSessionStateModel.Status = SessionStatus.Success;
    }
    

    private async Task<SessionStateModel> GetOrCreateSession(ulong sessionId)
    {
        if (await _sessionStatesRepository.Exists(sessionId))
        {
            var session = await _sessionStatesRepository.GetById(sessionId);
            if (session.Status != SessionStatus.InProgress)
                throw new ArgumentException("Session by required ID is already COMPLETED!");
            return session;
        }
        else
        {
            var session = new SessionStateModel { Id = sessionId };
            await _sessionStatesRepository.Create(session);
            return session;
        }
    }
}