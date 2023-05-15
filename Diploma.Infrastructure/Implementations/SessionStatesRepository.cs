using Diploma.Domain.Entities;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;

public class SessionStatesRepository : BaseRepository<SessionStateModel>, ISessionStatesRepository
{
    public SessionStatesRepository(ApplicationDbContext db) : base(db)
    {

    }

    public override async Task Create(SessionStateModel entity)
    {
        await _db.Sessions.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public override async Task<SessionStateModel> GetById(ulong id)
    {
        return await _db.Sessions.FindAsync(id) ?? throw new ArgumentException("Нет сессии с заданным id");
    }

    public override async Task<IEnumerable<SessionStateModel>> GetAll()
    {
        return await _db.Sessions.ToListAsync().ContinueWith(t => (IEnumerable<SessionStateModel>)t.Result);
    }

    public override async Task DeleteById(ulong id)
    {
        var session = await GetById(id);
        _db.Sessions.Remove(session);
        await _db.SaveChangesAsync();
    }

    public override async Task Update(SessionStateModel sessionStateModel)
    {
        _db.Sessions.Update(sessionStateModel);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Exists(ulong id)
    {
        return await _db.Sessions.FindAsync(id) is not null;
    }
}