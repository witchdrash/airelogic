using System.Collections.Generic;
using System.Security.Permissions;
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
            return new []{ new Artist(artistName, null), };
        }
    }

    public class Artist
    {
        public Artist(string name, string id)
        {
            Name = name;
            Id = id;
        }
        public string Id { get; }
        public string Name { get; }
    }
}