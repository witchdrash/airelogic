using LyricInfoApi.Models;
using LyricInfoApi.Repositories;

namespace LyricInfoApi.Services
{
    public class LyricStatisticsService : ILyricStatisticsService
    {
        private readonly IArtistRepository _artistRepository;

        public LyricStatisticsService(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public LyricStatistics CalculateFor(string expectedFor)
        {
            return null;
        }
    }
}