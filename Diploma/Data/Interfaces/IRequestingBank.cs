namespace Diploma.Data.Interfaces
{
    public interface IRequestingBank
    {
        string CalculatePSign(IDictionary<string, string> model);
        IDictionary<string, string> SetRequestingModel(IDictionary<string, object?> model);
    }
}
