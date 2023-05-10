using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Infrastructure.Implementations;

public class RecurPaymentResponsesRepository : BaseResponsesRepository<RecurOperationResponse>, IResponsesRepository<RecurOperationResponse>
{
    public RecurPaymentResponsesRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
    {
        
    }
    
    public override async Task Create(RecurOperationResponse entity)
    {
        await _db.RecurOperationResponses.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public override async Task<IEnumerable<RecurOperationResponse>> GetAll()
    {
        return await _db.RecurOperationResponses.ToListAsync();
    }
}