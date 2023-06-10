namespace Diploma.Application.Implementations
{
    /// <summary>
    /// Клиент для отправки запросов к API банка.
    /// </summary>
    internal class BankHttpClient
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client = new();

        internal BankHttpClient(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        /// <summary>
        /// Отправляет модель данных в банк и возвращает ответ, полученный от банка.
        /// </summary>
        internal async Task<HttpResponseMessage> SendModelToBankAsync(IDictionary<string, string> model)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
            request.Content = new FormUrlEncodedContent(model);
            var response = await _client.SendAsync(request);
            return response;
        }
    }
}