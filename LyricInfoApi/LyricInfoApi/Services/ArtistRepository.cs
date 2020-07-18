using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using LyricInfoApi.Models;

namespace LyricInfoApi.Services
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public ArtistRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public ICollection<Artist> SearchFor(string artistName)
        {
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://musicbrainz.org/ws/2/artist/?query=artist:{artistName}");
            request.Headers.Add("User-Agent", "AireLogicTechnicalTest/0.0.1");
            request.Headers.Add("Accept", "application/json");

            var response = client.SendAsync(request).Result;

            var musicBrainzArtistsCollection = JsonSerializer.Deserialize<MusicBrainzArtistsCollection>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            return musicBrainzArtistsCollection.Artists;
        }
    }
}