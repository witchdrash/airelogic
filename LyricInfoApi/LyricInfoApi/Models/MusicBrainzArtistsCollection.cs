using System.Collections.Generic;

namespace LyricInfoApi.Models
{
    public class MusicBrainzArtistsCollection
    {
        public ICollection<Artist> Artists { get; set; }
    }
}