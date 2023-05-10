using Diploma.Domain.Responses;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;

namespace Diploma.Application.Interfaces
{
    public interface IFiscalizePaymentService
    {
        Task<FiscalPaymentResponse> FiscalizePayment(FiscalReceiptDto receiptDto, RecurringPaymentModel lastPaymentModel);
    }
}
