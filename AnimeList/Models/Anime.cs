namespace AnimeList.Models
{
    public enum AnimeType
    {
        TV,
        Movie,
        OVA,
        ONA,
        Special,
        Music,
        Unknown
    }

    public enum AnimeSeason
    {
        Winter,
        Spring,
        Summer,
        Fall
    }
    public class Anime : BaseEntity
    {
        public int MalId { get; set; }
        public Title Titles { get; set; } = new();
        public Description Descriptions { get; set; } = new();
        public AnimeType Type { get; set; }
        public string[]? TrailerUrls { get; set; }
        public string? ImageUrl { get; set; }
        public int? Year { get; set; }
        public AnimeSeason? Season { get; set; }
        public int? Episodes { get; set; }
        public int? Duration { get; set; }
        public float? MalScore { get; set; }
        public int? FanSubId { get; set; }
        public List<FanSub> FanSubs { get; set; } = [];
        public List<Genre> Genres { get; set; } = [];
        public List<Studio> Studios { get; set; } = [];
    }

    public class Title
    {
        public string JpRomaji { get; set; } = string.Empty;
        public string JpKanji { get; set; } = string.Empty;
        public string? En { get; set; }
    }

    public class Description
    {
        public string En { get; set; } = string.Empty;
        public string? Hu { get; set; }
    }
}
