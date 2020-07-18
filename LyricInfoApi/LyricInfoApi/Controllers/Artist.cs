namespace LyricInfoApi.Controllers
{
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