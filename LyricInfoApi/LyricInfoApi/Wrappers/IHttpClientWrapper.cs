namespace LyricInfoApi.Wrappers
{
    public interface IHttpClientWrapper
    {
        T GetResponse<T>();
    }
}