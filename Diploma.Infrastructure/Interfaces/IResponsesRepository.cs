namespace Diploma.Infrastructure.Interfaces;

public interface IResponsesRepository<T> where T: Diploma.Domain.Responses.BaseResponse
{
    Task Create(T entity);
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(ulong id);
}