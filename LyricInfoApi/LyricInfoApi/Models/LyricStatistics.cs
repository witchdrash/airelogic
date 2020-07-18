namespace LyricInfoApi.Models
{
    public class LyricStatistics
    {
        public string For { get; set; }
        public double AverageLyricsPerSong { get; set; }
        public int UnFoundSongs { get; set; }
        public int FoundSongs { get; set; }
    }
}