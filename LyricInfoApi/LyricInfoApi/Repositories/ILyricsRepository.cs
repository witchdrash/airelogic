namespace LyricInfoApi.Repositories
{
    public interface ILyricsRepository
    {
        string GetFor(string artist, string song);
    }
}