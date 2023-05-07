namespace Diploma.Infastructure.Interfaces;

public interface IBaseRepository<T>
{
    void Add(ulong id, T? value);
    T Get(ulong id);
    bool Contains(ulong id);
    void Remove(ulong id);
}