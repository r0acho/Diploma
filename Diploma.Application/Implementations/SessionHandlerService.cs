using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Domain.Responses;

namespace Diploma.Application.Implementations;

public class SessionHandlerService : ISessionHandlerService
{
    private const string SUCCESS_BANK_RESPONSE_CODE = "00";
    private const string SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE = "96";
    private const string SESSION_ABORTED = "Session aborted";
    private const string SESSION_SUCCESSFULLY = "Session completed successfully";
    private const int INTERMEDIATE_SESSION_COST = 50;
    private const int COST_OF_ONE_KWH = 16;
    
    private List<ItemFiscalReceiptDto> _items = new();
    private readonly BankSettings _bankSettings;
    private SessionStatus _status = SessionStatus.InProgress;
    private IFiscalizePaymentService _fiscalizePaymentService;
    private IPaymentService _paymentService;
    
    private decimal _sumOfSessionsByBank = 0;
    private decimal _sumOfSessionsByTOUCH = 0;
    
    public SessionHandlerService(BankSettings bankSettings, IFiscalizePaymentService fiscalizePaymentService)
    {
        _fiscalizePaymentService = fiscalizePaymentService;
        _bankSettings = bankSettings;
        _paymentService = new PaymentService(_bankSettings);
    }

    public SessionHandlerService(BankSettings bankSettings, 
        IFiscalizePaymentService fiscalizePaymentService, 
        IBankOperationService service) : this(bankSettings, fiscalizePaymentService)
    {
        _paymentService = new PaymentService(_bankSettings, service);
    }

    private bool IsPaymentCompletedWithoutError(RecurOperationResponse response)
    {
        return response.ResponseCode == SUCCESS_BANK_RESPONSE_CODE 
               || response.ResponseCode == SYSTEM_MALFUNCTION_BANK_RESPONSE_CODE;
    }

    private SessionResponse GetSessionResponse(RecurOperationResponse response)
    {
        return new SessionResponse
        {
            Description = IsPaymentCompletedWithoutError(response) ? SESSION_SUCCESSFULLY : SESSION_ABORTED,
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

    private void AddItemToReceipt(RecurringPaymentModel paymentModel)
    {
        _items.Add(new ItemFiscalReceiptDto
        {
            Description = paymentModel.Description,
            Price = (uint)paymentModel.Amount * 100,
            QtyDecimal = paymentModel.Amount / COST_OF_ONE_KWH,
            TaxId = 1, //узнать процент налога
            PayAttribute = 4 //какой нужен?
        });
    }
    
    public async IAsyncEnumerable<BaseResponse> StartRecurringPayment(RecurringPaymentModel paymentModel)
    {
        _sumOfSessionsByTOUCH += paymentModel.Amount;
        var operationResponse = await _paymentService.ExecuteRecurringPayment(paymentModel);

        if (IsPaymentCompletedWithoutError(operationResponse))
        {
            _sumOfSessionsByBank += operationResponse.Amount;
            paymentModel.Status = PaymentStatus.Accepted;
            AddItemToReceipt(paymentModel);
        }
        else
        {
            paymentModel.Status = PaymentStatus.Cancelled;
        }
        yield return operationResponse;
        if (paymentModel.Amount != INTERMEDIATE_SESSION_COST)
        {
            
            yield return GetSessionResponse(operationResponse);
            //стучимся в АТОЛ
            yield return await Fiscalize(paymentModel);
        }
    }

    private async Task<FiscalPaymentResponse> Fiscalize(RecurringPaymentModel paymentModel)
    {
        var fiscalReceiptDto = new FiscalReceiptDto
        {
            PhoneOrEmail = paymentModel.Email!, TaxMode = 1,
            Items = _items, NonCash = new [] { (uint)_sumOfSessionsByBank * 100 },
            Place = paymentModel.ModuleUrl!
        };

        return await _fiscalizePaymentService.FiscalizePayment(fiscalReceiptDto, paymentModel);
    }
}