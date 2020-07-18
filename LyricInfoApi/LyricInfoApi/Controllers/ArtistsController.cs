using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace LyricInfoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistsController : ControllerBase
    {
        [HttpPost("search")]
        public IEnumerable<Artist> Search(string artistName)
        {
            return new []{ new Artist(artistName), };
        }
    }

    public class Artist
    {
        public Artist(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}