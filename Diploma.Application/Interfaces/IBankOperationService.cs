namespace Diploma.Application.Interfaces;

public interface IBankOperationService
{
    string CalculatePSignOfModel(Dictionary<string, string> model);
    IDictionary<string, string> GetRequestingModel();
}