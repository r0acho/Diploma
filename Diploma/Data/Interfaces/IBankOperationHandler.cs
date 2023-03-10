namespace Diploma.Data.Interfaces;

public interface IBankOperationHandler
{
    bool CheckKeys(Dictionary<string, string> bankResponse);

    Task<string> GetJsonResult();

}