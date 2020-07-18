using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using LyricInfoApi.Models;
using LyricInfoApi.Wrappers;

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
}