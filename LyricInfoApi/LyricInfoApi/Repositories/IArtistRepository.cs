using System.Collections.Generic;
using LyricInfoApi.Models;

namespace LyricInfoApi.Repositories
{
    public interface IArtistRepository
    {
        ICollection<Artist> SearchFor(string artistName);
    }
}