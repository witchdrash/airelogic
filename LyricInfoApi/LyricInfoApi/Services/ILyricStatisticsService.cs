using LyricInfoApi.Models;

namespace LyricInfoApi.Services
{
    public interface ILyricStatisticsService
    {
        LyricStatistics CalculateFor(string expectedFor);
    }
}