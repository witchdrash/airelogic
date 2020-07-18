using System.Linq;
using LyricInfoApi.Models;
using LyricInfoApi.Repositories;

namespace LyricInfoApi.Services
{
    public class LyricStatisticsService : ILyricStatisticsService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly ILyricsRepository _lyricsRepository;

        public LyricStatisticsService(IArtistRepository artistRepository, ILyricsRepository lyricsRepository)
        {
            _artistRepository = artistRepository;
            _lyricsRepository = lyricsRepository;
        }

        public LyricStatistics CalculateFor(string artistId)
        {
            var works = _artistRepository.GetWorksFor(artistId);
            var unfoundSongs = 0;
            
            if (works == null)
            {
                return null;
            }

            var average = works.Works.Average(x =>
            {
                var lyrics = _lyricsRepository.GetFor(works.ArtistName, x.Title);

                if (lyrics == null)
                {
                    unfoundSongs++;
                    return 0;
                }
                
                return lyrics.Split(" ").Length;
            });
            
            return new LyricStatistics
            {
                For = works.ArtistName,
                AverageLyricsPerSong = average,
                UnFoundSongs = unfoundSongs,
                FoundSongs = works.Works.Count - unfoundSongs
            };
        }
    }
}