using Diploma.Application.Implementations.BankOperations;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;

namespace Diploma.Application.Interfaces;

public interface IBankOperationService
{
    string CalculatePSignOfModel(Dictionary<string, string> model);
    IDictionary<string, string> GetRequestingModel();
}