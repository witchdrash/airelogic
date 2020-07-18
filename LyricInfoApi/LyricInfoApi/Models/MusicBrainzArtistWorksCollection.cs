using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LyricInfoApi.Models
{
    public class MusicBrainzArtistWorksCollection
    {
        public ICollection<Works> Works { get; set; } = new List<Works>();
        [JsonPropertyName("work-count")]
        public int WorkCount { get; set; }
        [JsonPropertyName("work-offset")]
        public int WorkOffset { get; set; }
    }
}