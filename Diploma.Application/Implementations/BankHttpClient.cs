using Diploma.Domain.Entities;

namespace Diploma.Application.Implementations;

internal class BankHttpClient
{
    private readonly HttpClient _client = new();
    private readonly string _apiUrl;

    internal BankHttpClient(string apiUrl)
    {
        _apiUrl = apiUrl;
    }
    internal async Task<HttpResponseMessage> SendModelToBankAsync(IDictionary<string, string> model, string url)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new FormUrlEncodedContent(model);
        var response = await _client.SendAsync(request);
        return response;
    }
}