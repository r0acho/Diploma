using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface IFiscalizePaymentService
{
    Task<FiscalizeResponse> FiscalizePayment(SessionStateModel sessionStateModel, RecurringPaymentModel lastPaymentModel, decimal costOfOneKwh);
}