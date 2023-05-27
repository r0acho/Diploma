using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;

public class RecurPaymentResponsesRepository : IResponsesRepository<RecurOperationResponse>
{
    private readonly ApplicationDbContext _db;
    public RecurPaymentResponsesRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Create(RecurOperationResponse entity)
    {
        await _db.Payments.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<RecurOperationResponse>> GetAll()
    {
        return await _db.Payments.ToListAsync();
    }

    public async Task<RecurOperationResponse> GetById(ulong id)
    {
        return await _db.Payments.FindAsync(id) ??
               throw new ArgumentException("Нет ответа по платежу с заданным id");
        ;
    }
}