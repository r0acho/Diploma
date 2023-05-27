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

    [HttpGet("GetPaymentResponses")]
    public async Task<IEnumerable<RecurOperationResponse>> GetAllPaymentResponses()
    {
        return await _paymentInformationService.GetRecurPaymentResponses();
    }

    [HttpGet("GetPaymentResponseById")]
    public async Task<RecurOperationResponse> GetPaymentResponseById(ulong id)
    {
        return await _paymentInformationService.GetRecurPaymentResponseById(id);
    }
}