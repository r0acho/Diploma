using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Infrastructure.Implementations;

public class FiscalResponsesRepository : BaseResponsesRepository<FiscalPaymentResponse>, IResponsesRepository<FiscalPaymentResponse>
{
    public FiscalResponsesRepository(ApplicationDbContext db) : base(db)
    {
        
    }
    public override async Task Create(FiscalPaymentResponse entity)
    {
        _db.FiscalPaymentResponses.Add(entity);
        await _db.SaveChangesAsync();
    }

    public override async Task<IEnumerable<FiscalPaymentResponse>> GetAll()
    {
        return await _db.FiscalPaymentResponses.ToListAsync();
    }
}