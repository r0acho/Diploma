using Diploma.Domain.Responses;

namespace Diploma.Infrastructure.Interfaces;

public interface IChecksRepository
{
    Task Create(FiscalizeResponse entity);
    Task<IEnumerable<FiscalizeResponse>> GetAll();
    Task<FiscalizeResponse> GetById(ulong id);
}