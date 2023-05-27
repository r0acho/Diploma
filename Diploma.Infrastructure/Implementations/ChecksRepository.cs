using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;

public class ChecksRepository : IChecksRepository
{
    private readonly ApplicationDbContext _db;
    public ChecksRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Create(FiscalizeResponse entity)
    {
        _db.Checks.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<FiscalizeResponse>> GetAll()
    {
        return await _db.Checks.ToListAsync();
    }

    public async Task<FiscalizeResponse> GetById(ulong id)
    {
        return await _db.Checks.FindAsync(id) ??
               throw new ArgumentException("Нет ответа по чеку с заданным id");
    }
}