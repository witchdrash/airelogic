using LyricInfoApi.Wrappers;

namespace LyricInfoApi.Repositories
{
    public interface ILyricsRepository
    {
        string GetFor(string artist, string song);
    }

    public class LyricsRepository : ILyricsRepository
    {
        private readonly IHttpClientFactoryWrapper _clientFactory;

        public LyricsRepository(IHttpClientFactoryWrapper clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        public string GetFor(string artist, string song)
        {
            return null;
        }
    }
}