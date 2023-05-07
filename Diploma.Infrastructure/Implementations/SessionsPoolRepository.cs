using Diploma.Infrastructure.Interfaces;

namespace Diploma.Infrastructure.Implementations;

public class SessionsPoolRepository<T> : IDictBaseRepository<T>
{
    private readonly IDictionary<ulong, T?> _repository =
        new Dictionary<ulong, T?>();


    public bool Add(ulong id, T? value)
    {
        return _repository.TryAdd(id, value);
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

    public bool Remove(ulong id)
    {
        return _repository.Remove(id);
    }

}