namespace AnimeList.Models
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public List<Anime> Animes { get; set; } = [];
    }
}
