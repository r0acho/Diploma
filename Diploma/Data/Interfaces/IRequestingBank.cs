namespace Diploma.Data.Interfaces
{
    public interface IRequestingBank
    {
        string CalculatePSign(IDictionary<string, object> model);
        IDictionary<string, object> PrepareSendingData(IDictionary<string, object> model);
    }
}
