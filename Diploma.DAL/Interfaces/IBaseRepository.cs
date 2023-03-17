namespace Diploma.DAL.Interfaces;

public interface IBaseRepository<T>
{
    // event - ..Handler
    bool Create(T entity);

    T Get(int id);
    
}