using System.Net.Http;
using System.Text.Json;

namespace LyricInfoApi.Wrappers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _createClient;
        private HttpRequestMessage _request;

        public HttpClientWrapper(HttpClient createClient, HttpMethod httpMethod, string url)
        {
            _createClient = createClient;
            _request = new HttpRequestMessage(httpMethod, url);
            _request.Headers.Add("User-Agent", "AireLogicTechnicalTest/0.0.1");
            _request.Headers.Add("Accept", "application/json");
        }

        public T GetResponse<T>()
        {
            var response = _createClient.SendAsync(_request).Result;

            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result,
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }
    }
}