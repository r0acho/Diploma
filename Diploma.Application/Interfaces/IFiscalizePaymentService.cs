using Diploma.Domain.Responses;
using Diploma.Domain.Dto;

namespace Diploma.Application.Interfaces
{
    public interface IFiscalizePaymentService
    {
        FiscalPaymentResponse FiscalizePayment(FiscalReceiptDto receiptDto);
    }
}
