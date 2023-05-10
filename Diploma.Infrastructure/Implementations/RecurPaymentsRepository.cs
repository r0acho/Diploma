using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Infrastructure.Implementations;

public class RecurPaymentsRepository : IRecurPaymentsRepository
{
    private readonly ApplicationDbContext _db;
    
    public RecurPaymentsRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task Create(RecurringPaymentModel entity)
    {
        await _db.RecurringPaymentModels.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<RecurringPaymentModel> GetById(ulong id)
    {
        return await _db.RecurringPaymentModels.FindAsync(id) ?? throw new ArgumentException("Нет платежа с заданным id");
    }

    public async Task<IEnumerable<RecurringPaymentModel>> GetAll()
    {
        return await _db.RecurringPaymentModels.ToListAsync().ContinueWith(t => 
            (IEnumerable<RecurringPaymentModel>)t.Result);
    }

    public async Task DeleteById(ulong id)
    {
        var payment = await GetById(id);
        _db.RecurringPaymentModels.Remove(payment);
        await _db.SaveChangesAsync();
    }

    public async Task Update(RecurringPaymentModel entity)
    {
        _db.RecurringPaymentModels.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<PaymentStatus> GetStatus(ulong id)
    {
        var payment = await GetById(id);
        return payment.Status;
    }
}