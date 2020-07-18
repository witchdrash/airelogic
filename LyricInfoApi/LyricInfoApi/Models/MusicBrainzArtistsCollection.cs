using System.Collections.Generic;
using LyricInfoApi.Models;

namespace LyricInfoApi.Services
{
    public class MusicBrainzArtistsCollection
    {
        public ICollection<Artist> Artists { get; set; }
    }
}