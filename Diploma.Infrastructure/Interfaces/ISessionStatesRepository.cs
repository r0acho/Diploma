using Diploma.Domain.Entities;

namespace Diploma.Infrastructure.Interfaces;

public interface ISessionStatesRepository : IBaseRepository<SessionStateModel>
{
    Task<bool> Exists(ulong id);
}