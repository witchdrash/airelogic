using System.Collections.Generic;
using LyricInfoApi.Controllers;
using LyricInfoApi.Models;

namespace LyricInfoApi.Services
{
    public interface IArtistRepository
    {
        ICollection<Artist> SearchFor(string artistName);
    }

    public interface ILyricStatisticsService
    {
        LyricStatistics CalculateFor(string expectedFor);
    }

    public class LyricStatisticsService : ILyricStatisticsService
    {
        public LyricStatistics CalculateFor(string expectedFor)
        {
            throw new System.NotImplementedException();
        }
    }
    
}