using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using LyricInfoApi.Models;

namespace LyricInfoApi.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IHttpClientFactoryWrapper _clientFactory;

        public ArtistRepository(IHttpClientFactoryWrapper clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public ICollection<Artist> SearchFor(string artistName)
        {
            var client = _clientFactory.CreateClient(HttpMethod.Get, $"http://musicbrainz.org/ws/2/artist/?query=artist:{artistName}");

            return client.GetResponse<MusicBrainzArtistsCollection>().Artists;
        }
    }

    public interface IHttpClientFactoryWrapper
    {
        IHttpClientWrapper CreateClient(HttpMethod get, string s);
    }

    public class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientFactoryWrapper(IHttpClientFactory _clientFactory)
        {
            this._clientFactory = _clientFactory;
        }

        public IHttpClientWrapper CreateClient(HttpMethod httpMethod, string url)
        {
            return new HttpClientWrapper(_clientFactory.CreateClient(), httpMethod, url);
        }
    }

    public interface IHttpClientWrapper
    {
        T GetResponse<T>();
    }

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