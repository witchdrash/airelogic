using System.Collections.Generic;
using LyricInfoApi.Models;
using LyricInfoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LyricInfoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;
        private readonly ILyricStatisticsService _lyricStatisticsService;

        public ArtistsController(IArtistRepository artistRepository, ILyricStatisticsService lyricStatisticsService)
        {
            _artistRepository = artistRepository;
            _lyricStatisticsService = lyricStatisticsService;
        }
        
        [HttpPost("search")]
        public ICollection<Artist> Search(string artistName)
        {
            return _artistRepository.SearchFor(artistName);
        }

        [HttpGet("{id}")]
        public LyricStatistics GetLyricStats(string id)
        {
            return null;
        }
    }

    public class LyricStatistics
    {
        public string For { get; set; }
        public decimal AverageLyricsPerSong { get; set; }
    }
}