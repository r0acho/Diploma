using Diploma.Domain.Entities;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;

/// <summary>
/// Реализация репозитория состояний сессий в базе данных.
/// </summary>
public class SessionStatesRepository : ISessionStatesRepository
{
    private readonly ApplicationDbContext _db;
    
    /// <summary>
    /// Создает новый экземпляр репозитория состояний сессий.
    /// </summary>
    /// <param name="db">Экземпляр базы данных.</param>
    public SessionStatesRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task Create(SessionStateModel entity)
    {
        await _db.Sessions.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<SessionStateModel> GetById(ulong id)
    {
        return await _db.Sessions.FindAsync(id) ?? throw new ArgumentException("Нет сессии с заданным id");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<SessionStateModel>> GetAll()
    {
        return await _db.Sessions.ToListAsync().ContinueWith(t => (IEnumerable<SessionStateModel>)t.Result);
    }

    /// <inheritdoc/>
    public async Task DeleteById(ulong id)
    {
        var session = await GetById(id);
        _db.Sessions.Remove(session);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Update(SessionStateModel sessionStateModel)
    {
        _db.Sessions.Update(sessionStateModel);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> Exists(ulong id)
    {
        return await _db.Sessions.FindAsync(id) is not null;
    }
}