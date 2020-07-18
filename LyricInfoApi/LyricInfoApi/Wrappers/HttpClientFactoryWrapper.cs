using System.Net.Http;

namespace LyricInfoApi.Wrappers
{
    public class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientFactoryWrapper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IHttpClientWrapper CreateClient(HttpMethod httpMethod, string url)
        {
            return new HttpClientWrapper(_clientFactory.CreateClient(), httpMethod, url);
        }
    }
}