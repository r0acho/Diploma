using Diploma.Application.Interfaces;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Application.Implementations;

public class CheckInformationService : ICheckInformationService
{
    private readonly IChecksRepository _checksRepository;

    public CheckInformationService(IChecksRepository checksRepository)
    {
        _checksRepository = checksRepository;
    }
    
    public async Task<IEnumerable<FiscalizeResponse>> GetChecks()
    {
        return await _checksRepository.GetAll();
    }

    public async Task<FiscalizeResponse> GetCheckById(ulong id)
    {
        return await _checksRepository.GetById(id);
    }

    public async Task<FiscalizeResponse> GetCheckByUuId(string uuid)
    {
        var checks =  await _checksRepository.GetAll();
        return checks.FirstOrDefault(x => x.Uuid == uuid) ?? throw new ArgumentException("Нет чека с заданным uuid");
    }
}