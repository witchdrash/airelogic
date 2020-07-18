using System.Collections.Generic;
using LyricInfoApi.Models;

namespace LyricInfoApi.Services
{
    public class ArtistRepository : IArtistRepository
    {
        public ICollection<Artist> SearchFor(string artistName)
        {
            return null;
        }
    }
}