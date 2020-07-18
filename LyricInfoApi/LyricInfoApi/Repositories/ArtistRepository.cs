using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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
            var currentOffset = 0;
            var getNextPage = true;

            var toReturn = new List<Works>();
            
            while (getNextPage)
            {
                var client = _clientFactory.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset={currentOffset}");
                var result = client.GetResponse<MusicBrainzArtistWorksCollection>();
                if (result == null)
                {
                    return null;
                }
                
                toReturn.AddRange(result.Works);
                if (currentOffset + 100 > result.WorkCount)
                {
                    getNextPage = false;
                }
                
                currentOffset+=100;

                if (getNextPage)
                {
                    Thread.Sleep(1000); // Avoid the rate limiter and banning
                }
            }

            return toReturn;
        }
    }
}