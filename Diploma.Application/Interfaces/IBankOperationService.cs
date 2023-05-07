using Diploma.Domain.Entities;

namespace Diploma.Application.Interfaces;

internal interface IBankOperationService
{
    string CalculatePSignOfModel(Dictionary<string, string> model);
    IDictionary<string, string> GetRequestingModel(BankOperation model);
}