using System.Collections.Generic;
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
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://musicbrainz.org/ws/2/artist/?query=artist:{artistName}");

            return client.GetResponse<MusicBrainzArtistsCollection>(request).Artists;
        }
    }

    public interface IHttpClientFactoryWrapper
    {
        IHttpClientWrapper CreateClient();
    }

    public class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientFactoryWrapper(IHttpClientFactory _clientFactory)
        {
            this._clientFactory = _clientFactory;
        }

        public IHttpClientWrapper CreateClient()
        {
            return new HttpClientWrapper(_clientFactory.CreateClient());
        }
    }

    public interface IHttpClientWrapper
    {
        T GetResponse<T>(HttpRequestMessage request);
    }

    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _createClient;

        public HttpClientWrapper(HttpClient createClient)
        {
            _createClient = createClient;
        }

        public T GetResponse<T>(HttpRequestMessage request)
        {
            request.Headers.Add("User-Agent", "AireLogicTechnicalTest/0.0.1");
            request.Headers.Add("Accept", "application/json");

            var response = _createClient.SendAsync(request).Result;

            return JsonSerializer.Deserialize<T>(response.Content.ReadAsStringAsync().Result,
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }
    }
}