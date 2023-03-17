namespace Diploma.Service.Interfaces;

public interface ISessionHandlerService
{
    bool CheckKeys(Dictionary<string, string> bankResponse);

    Task<string> GetJsonResult();
}