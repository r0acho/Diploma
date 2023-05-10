using Diploma.Domain.Responses;
using Diploma.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Diploma.Infrastructure.Implementations;

public abstract class BaseResponsesRepository<T> : IResponsesRepository<T> where T : BaseResponse
{
    protected readonly ApplicationDbContext _db;
    
    protected BaseResponsesRepository(IServiceScopeFactory serviceScopeFactory)
    {
        _db = serviceScopeFactory.CreateScope().ServiceProvider.GetService<ApplicationDbContext>() ??
              throw new InvalidOperationException("Невозможно создать контекст базы данных");
    }

    public abstract Task Create(T entity);
    public abstract Task<IEnumerable<T>> GetAll();
}