namespace Diploma.Data.Interfaces
{
    public interface IRequestingBank
    {
        string CalculatePSign(IDictionary<string, object> model);
        IDictionary<string, object> SetRequestingModel(IDictionary<string, object> model);
    }
}
