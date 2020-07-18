using System.Collections.Generic;
using System.Security.Permissions;
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

        public ArtistsController(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }
        
        [HttpPost("search")]
        public ICollection<Artist> Search(string artistName)
        {
            return _artistRepository.SearchFor(artistName);
        }
    }
}