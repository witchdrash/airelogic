using System.Text.Json.Serialization;

namespace LyricInfoApi.Models
{
    public class Works
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Disambiguation { get; set; }
    }
}