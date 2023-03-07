namespace Diploma.Data.Interfaces
{
    public interface IBankOperations
    {
        string CalculatePSignOfModel(Dictionary<string, string> model);
        IDictionary<string, string> SetRequestingModel(IDictionary<string, object?> model);
    }
}
