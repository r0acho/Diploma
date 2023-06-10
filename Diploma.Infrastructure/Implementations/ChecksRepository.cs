using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;
 
/// <summary>
/// Реализация репозитория фискальных чеков в базе данных.
/// </summary>
public class ChecksRepository : IChecksRepository
{
    private readonly ApplicationDbContext _db;
    
    /// <summary>
    /// Создает новый экземпляр репозитория фискальных чеков.
    /// </summary>
    /// <param name="db">Экземпляр базы данных.</param>
    public ChecksRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task Create(FiscalizeResponse entity)
    {
        _db.Checks.Add(entity);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<FiscalizeResponse>> GetAll()
    {
        return await _db.Checks.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<FiscalizeResponse> GetById(ulong id)
    {
        return await _db.Checks.FindAsync(id) ??
               throw new ArgumentException("Нет ответа по чеку с заданным id");
    }
}