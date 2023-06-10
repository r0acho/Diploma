using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;

/// <summary>
/// Реализация репозитория результатов рекуррентных платежей в базе данных.
/// </summary>
public class RecurPaymentResponsesRepository : IResponsesRepository<RecurOperationResponse>
{
    private readonly ApplicationDbContext _db;
    
    /// <summary>
    /// Создает новый экземпляр репозитория результатов рекуррентных платежей.
    /// </summary>
    /// <param name="db">Экземпляр базы данных.</param>
    public RecurPaymentResponsesRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task Create(RecurOperationResponse entity)
    {
        await _db.Payments.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RecurOperationResponse>> GetAll()
    {
        return await _db.Payments.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<RecurOperationResponse> GetById(ulong id)
    {
        return await _db.Payments.FindAsync(id) ??
               throw new ArgumentException("Нет ответа по платежу с заданным id");
        ;
    }
}