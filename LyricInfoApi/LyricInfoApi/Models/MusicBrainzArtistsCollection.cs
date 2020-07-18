using System.Collections.Generic;

namespace LyricInfoApi.Models
{
    public class MusicBrainzArtistsCollection
    {
        public ICollection<Artist> Artists { get; set; } = new List<Artist>();
    }

    public class MusicBrainzArtistWorksCollection
    {
        public ICollection<Works> Works { get; set; } = new List<Works>();
        public int WorkCount { get; set; }
        public int WorkOffset { get; set; }
    }

    public class Works
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}