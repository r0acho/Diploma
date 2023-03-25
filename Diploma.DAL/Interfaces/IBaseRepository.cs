namespace Diploma.DAL.Interfaces;

public interface IBaseRepository<T>
{
    bool Create(T entity);

    T Get(ulong id);

    bool Delete(ulong id);
}