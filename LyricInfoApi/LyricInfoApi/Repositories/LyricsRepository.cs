using System.Net.Http;
using LyricInfoApi.Models;
using LyricInfoApi.Wrappers;

namespace LyricInfoApi.Repositories
{
    public class LyricsRepository : ILyricsRepository
    {
        private readonly IHttpClientFactoryWrapper _clientFactory;

        public LyricsRepository(IHttpClientFactoryWrapper clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        public string GetFor(string artist, string song)
        {
            var client = _clientFactory.CreateClient(HttpMethod.Get,
                $"https://api.lyrics.ovh/v1/{artist}/{song}");
            var result = client.GetResponse<LyricsApiResponse>();

            return result?.Lyrics;
        }
    }
}