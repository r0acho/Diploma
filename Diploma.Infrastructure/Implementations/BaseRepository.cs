using Diploma.Infastructure.Interfaces;

namespace Diploma.DAL.Implementations;

public class BaseRepository<T> : IBaseRepository<T>
{
    private readonly IDictionary<ulong, T?> _repository =
        new Dictionary<ulong, T?>();


    public void Add(ulong id, T? value)
    {
        _repository.Add(id, value);
    }
    
    public T Get(ulong id)
    {
        return _repository.TryGetValue(id, out T? value) 
            ? value!
            : throw new KeyNotFoundException();
    }

    public bool Contains(ulong id)
    {
        return _repository.ContainsKey(id);
    }
    
    public void Remove(ulong id)
    {
        _repository.Remove(id);
    }
    
}