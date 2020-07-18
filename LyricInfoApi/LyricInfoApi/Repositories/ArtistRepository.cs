using System.Collections.Generic;
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

        public ICollection<Works> GetWorksFor(string artistId)
        {
            var client = _clientFactory.CreateClient(HttpMethod.Get,
                $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=0");
            var result = client.GetResponse<MusicBrainzArtistWorksCollection>();

            return result?.Works;
        }
    }
}