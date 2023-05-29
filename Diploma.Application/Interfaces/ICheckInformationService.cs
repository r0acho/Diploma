using Diploma.Domain.Responses;

namespace Diploma.Application.Interfaces;

public interface ICheckInformationService
{
    Task<IEnumerable<FiscalizeResponse>> GetChecks();
    Task<FiscalizeResponse> GetCheckById(ulong id);
    Task<FiscalizeResponse> GetCheckByUuId(string uuid);
}