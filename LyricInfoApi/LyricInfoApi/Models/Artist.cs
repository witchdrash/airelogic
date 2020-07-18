using System.Text.Json.Serialization;

namespace LyricInfoApi.Models
{
    public class Artist
    {
        public string Id { get; set;}
        public string Name { get; set; }
        
        // Added to keep consistency with MusicBrainz
        [JsonPropertyName("sort-name")]
        public string SortName { get; set; }
    }
}