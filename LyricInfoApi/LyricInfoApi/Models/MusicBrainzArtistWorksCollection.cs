using System.Collections.Generic;

namespace LyricInfoApi.Models
{
    public class MusicBrainzArtistWorksCollection
    {
        public ICollection<Works> Works { get; set; } = new List<Works>();
        public int WorkCount { get; set; }
        public int WorkOffset { get; set; }
    }
}