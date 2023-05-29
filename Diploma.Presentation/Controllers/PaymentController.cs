using Diploma.Application.Interfaces;
using Diploma.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentInformationService _paymentInformationService;

    public PaymentController(IPaymentInformationService paymentInformationService)
    {
        _paymentInformationService = paymentInformationService;
    }

    [HttpGet]
    [Route("GetPayments/")]
    public async Task<IEnumerable<RecurOperationResponse>> Payments()
    {
        return await _paymentInformationService.GetRecurPaymentResponses();
    }

    [HttpGet]
    [Route("GetPayments/{id}")]
    public async Task<RecurOperationResponse> Payments(ulong id)
    {
        return await _paymentInformationService.GetRecurPaymentResponseById(id);
    }
}