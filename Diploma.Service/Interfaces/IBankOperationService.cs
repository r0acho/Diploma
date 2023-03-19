using Diploma.Domain.Entities;

namespace Diploma.Service.Interfaces;

public interface IBankOperationService
{
    string CalculatePSignOfModel(Dictionary<string, string> model);
    IDictionary<string, string> SetRequestingModel(IDictionary<string, object?> model);
    IDictionary<string, string> SetRequestingModel(BankOperation model);
}