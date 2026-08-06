namespace AnimeList.Models
{
    public class Studio : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<Anime> Animes { get; set; } = [];
    }
}
