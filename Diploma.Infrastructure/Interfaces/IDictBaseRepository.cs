namespace Diploma.Infrastructure.Interfaces;

public interface IDictBaseRepository<T>
{
    bool Add(ulong id, T? value);
    T Get(ulong id);
    bool Contains(ulong id);
    bool Remove(ulong id);
}