namespace Diploma.Data.Interfaces
{
    public interface IRequestingBank
    {
        string CalculatePSign(IDictionary<string, object> model);
        void SetRequestingModel(IDictionary<string, object> model);
    }
}
