using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Infrastructure.Implementations;

public class FiscalResponsesRepository : BaseResponsesRepository<FiscalPaymentResponse>, IResponsesRepository<FiscalPaymentResponse>
{
    public FiscalResponsesRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
    {
        
    }
    public override async Task Create(FiscalPaymentResponse entity)
    {
        await _db.FiscalPaymentResponses.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public override async Task<IEnumerable<FiscalPaymentResponse>> GetAll()
    {
        return await _db.FiscalPaymentResponses.ToListAsync();
    }
}