using Diploma.DAL.Interfaces;
using Diploma.Domain.Entities;

namespace Diploma.DAL.Implementations;

public class BankOperationsRepository : IBankOperationsRepository
{
    private readonly List<BankOperation> _bankOperations = new();

    public BankOperationsRepository(ulong sessionId)
    {
        SessionId = sessionId;
    }

    public ulong SessionId { get; }

    public bool Create(BankOperation entity)
    {
        _bankOperations.Add(entity);
        return true;
    }

    public BankOperation Get(int id)
    {
        return _bankOperations[id];
    }
}