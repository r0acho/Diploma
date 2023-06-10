using Diploma.Application.Interfaces;
using Diploma.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

/// <summary>
/// API для получения информации о рекуррентных платежах из БД
/// </summary>
[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentInformationService _paymentInformationService;

    public PaymentController(IPaymentInformationService paymentInformationService)
    {
        _paymentInformationService = paymentInformationService;
    }

    /// <summary>
    /// Получить информацию о всех рекуррентных платежах из БД
    /// </summary>
    /// <returns>Список платежей</returns>
    [HttpGet]
    [Route("GetPayments/")]
    public async Task<IEnumerable<RecurOperationResponse>> Payments()
    {
        return await _paymentInformationService.GetRecurPaymentResponses();
    }

    /// <summary>
    /// Получить информацию о рекуррентном платеже по переданному ID
    /// </summary>
    /// <param name="id">ID платежа</param>
    /// <returns>Платеж по переданному ID</returns>
    [HttpGet]
    [Route("GetPayments/{id}")]
    public async Task<RecurOperationResponse> Payments(ulong id)
    {
        return await _paymentInformationService.GetRecurPaymentResponseById(id);
    }
}