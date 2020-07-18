using System.Collections.Generic;
using LyricInfoApi.Models;

namespace LyricInfoApi.Services
{
    public interface IArtistRepository
    {
        ICollection<Artist> SearchFor(string artistName);
    }
}