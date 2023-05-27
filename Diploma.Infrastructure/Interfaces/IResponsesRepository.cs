using Diploma.Domain.Responses;

namespace Diploma.Infrastructure.Interfaces;

public interface IResponsesRepository<T> where T : BaseResponse
{
    Task Create(T entity);
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(ulong id);
}