namespace Diploma.Service.Implementations;

internal class BankHttpClient
{
    private readonly HttpClient _client = new();
    
    internal async Task<HttpResponseMessage> SendModelToBankAsync(IDictionary<string, string> model)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, BankEnvironment.BankUrl);
        request.Content = new FormUrlEncodedContent(model);
        var response = await _client.SendAsync(request);
        return response;
    }
}