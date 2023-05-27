namespace Diploma.Infrastructure.Interfaces;

public interface IBaseRepository<T>
{
    Task Create(T entity);
    Task<T> GetById(ulong id);
    Task<IEnumerable<T>> GetAll();
    Task DeleteById(ulong id);
    Task Update(T entity);
}