using Diploma.Application.Interfaces;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Diploma.Application.Implementations;

/// <summary>
/// Реализация сервиса для обработки сессий платежей.
/// </summary>
public class SessionHandlerService : ISessionHandlerService
{
    /// <summary>
    /// Код успешного ответа от банка.
    /// </summary>
    private const string SUCCESS_BANK_RESPONSE_CODE = "00";

    /// <summary>
    /// Код ошибки системной неисправности.
    /// </summary>
    private const string SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE = "96";

    /// <summary>
    /// Стоимость промежуточной операции.
    /// </summary>
    private const int INTERMEDIATE_SESSION_COST = 50;

    /// <summary>
    /// Стоимость одного киловатт-часа.
    /// </summary>
    private const int COST_OF_ONE_KWH = 16;

    private readonly IFiscalizePaymentService _fiscalizePaymentService;
    private readonly IChecksRepository _checksRepository;

    private readonly ILogger<SessionHandlerService> _logger;
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
        if (recurringPayment.Amount is <= 0 or > INTERMEDIATE_SESSION_COST )
        {
            _logger.LogError($"Некорреткная сумма платежа {recurringPayment.Amount} в сессии {sessionId}");
        };
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

    /// <summary>
    /// Проверка успешности выполнения операции.
    /// </summary>
    /// <param name="response">Ответ от банка.</param>
    /// <returns>True, если операция была выполнена без ошибок. False в противном случае.</returns>
    private static bool IsPaymentCompletedWithoutError(RecurOperationResponse response)
    {
        return response.ResponseCode is SUCCESS_BANK_RESPONSE_CODE or SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE;
    }
    
    /// <summary>
    /// Добавление элемента в чек.
    /// </summary>
    /// <param name="paymentModel">Модель платежа.</param>
    /// <param name="items">Коллекция элементов на чеке.</param>
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

    /// <summary>
    /// Установка статуса сессии после завершения сессии.
    /// </summary>
    /// <param name="fiscalPaymentResponse">Ответ от сервиса фискализации на платеж в сессии.</param>
    private void SetSessionStatusAfterEndOfSession(FiscalizeResponse fiscalPaymentResponse)
    {
        if (_currentSessionStateModel.SumOfSessionsByBank != _currentSessionStateModel.SumOfSessionsByTouch)
        {
            _logger.LogWarning($"Ошибка сверки в сессии {_currentSessionStateModel!.Id}");
            _currentSessionStateModel.Status = SessionStatus.ReconciliationError;
        }
        else
        {
            _logger.LogInformation($"Сверка в сессии {_currentSessionStateModel!.Id} прошла успешно");
        }

        if (fiscalPaymentResponse.ErrorJson != null)
        {
            _logger.LogWarning($"Ошибка фискализации в сессии {_currentSessionStateModel!.Id}");
            _currentSessionStateModel.Status = SessionStatus.FiscalizationError;
        }
        else
        {
            _logger.LogInformation($"Фискализация чека в сессии {_currentSessionStateModel!.Id} прошла успешно");
        }

        if (_currentSessionStateModel.Status != SessionStatus.InProgress) return;
        _logger.LogInformation($"Сессия {_currentSessionStateModel!.Id} успешно завершена");
        _currentSessionStateModel.Status = SessionStatus.Success;
    }

    /// <summary>
    /// Получение текущей сессии или создание новой сессии с указанным ID.
    /// </summary>
    /// <param name="sessionId">ID сессии.</param>
    /// <returns>Существующая или созданная сессия.</returns>
    private async Task<SessionStateModel> GetOrCreateSession(ulong sessionId)
    {
        if (await _sessionStatesRepository.Exists(sessionId))
        {
            var session = await _sessionStatesRepository.GetById(sessionId);
            if (session.Status == SessionStatus.InProgress)
            {
                _logger.LogInformation("Из БД успешно извлеклась сессия с ID = {SessionId}", sessionId);
                return session;
            }
            string errorMessage = $"Сессия по переданному ID = {sessionId} уже завершена!";
            _logger.LogError(errorMessage);
            throw new ArgumentException(errorMessage);
        }
        else
        {
            var session = new SessionStateModel { Id = sessionId };
            await _sessionStatesRepository.Create(session);
            _logger.LogInformation("В БД создана сессия под ID {SessionId}", sessionId);
            return session;
        }
    }
}