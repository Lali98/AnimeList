namespace AnimeList.Models
{
    public class FanSub : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Link Links { get; set; } = new();

        public List<Anime> Animes { get; set; } = [];
    }

    public class Link
    {
        public string? Youtube { get; set; }
        public string? Discord { get; set; }
        public string? WebSite { get; set; }
        public string? IndaVideo { get; set; }
        public string? Videa { get; set; }

    }
}
