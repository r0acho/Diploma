using Diploma.Domain.Entities;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;

public class SessionStatesRepository : ISessionStatesRepository
{
    private readonly ApplicationDbContext _db;
    public SessionStatesRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Create(SessionStateModel entity)
    {
        await _db.Sessions.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<SessionStateModel> GetById(ulong id)
    {
        return await _db.Sessions.FindAsync(id) ?? throw new ArgumentException("Нет сессии с заданным id");
    }

    public async Task<IEnumerable<SessionStateModel>> GetAll()
    {
        return await _db.Sessions.ToListAsync().ContinueWith(t => (IEnumerable<SessionStateModel>)t.Result);
    }

    public async Task DeleteById(ulong id)
    {
        var session = await GetById(id);
        _db.Sessions.Remove(session);
        await _db.SaveChangesAsync();
    }

    public async Task Update(SessionStateModel sessionStateModel)
    {
        _db.Sessions.Update(sessionStateModel);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Exists(ulong id)
    {
        return await _db.Sessions.FindAsync(id) is not null;
    }
}