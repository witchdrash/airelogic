using System.Collections.Generic;
using LyricInfoApi.Models;

namespace LyricInfoApi.Repositories
{
    public class WorksCollection
    {
        public string ArtistName { get; set; }
        public ICollection<Works> Works { get; set; }
    }
}