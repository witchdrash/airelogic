using System.Net.Http;

namespace LyricInfoApi.Wrappers
{
    public interface IHttpClientFactoryWrapper
    {
        IHttpClientWrapper CreateClient(HttpMethod httpMethod, string url);
    }
}