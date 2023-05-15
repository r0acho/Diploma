using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Infrastructure.Implementations;

public class SessionResponsesRepository : BaseResponsesRepository<SessionResponse>, IResponsesRepository<SessionResponse>
{
    public SessionResponsesRepository(ApplicationDbContext db) : base(db)
    {
        
    }
    
    public override async Task Create(SessionResponse entity)
    {
        await _db.SessionResponses.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public override async Task<IEnumerable<SessionResponse>> GetAll()
    {
        return await _db.SessionResponses.ToListAsync();
    }
}