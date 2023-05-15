using Diploma.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Infrastructure.Implementations;

public abstract class BaseRepository<T> : IBaseRepository<T>
{
    protected readonly ApplicationDbContext _db;
    
    public BaseRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public abstract Task Create(T entity);
    public abstract Task<T> GetById(ulong id);
    public abstract Task<IEnumerable<T>> GetAll();
    public abstract Task DeleteById(ulong id);
    public abstract Task Update(T entity);
}