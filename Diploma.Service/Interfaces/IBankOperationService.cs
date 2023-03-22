using Diploma.Domain.Entities;

namespace Diploma.Service.Interfaces;

internal interface IBankOperationService
{
    string CalculatePSignOfModel(Dictionary<string, string> model);
    IDictionary<string, string> GetRequestingModel(BankOperation model);
}