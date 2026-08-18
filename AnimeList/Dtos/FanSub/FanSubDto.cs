namespace AnimeList.Dtos.FanSub
{
    public class FanSubDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public LinkDto Links { get; set; } = new();
        public List<int> AnimeIds { get; set; } = [];
    }

    public class LinkDto
    {
        public string? YouTube { get; set; }
        public string? Discord { get; set; }
        public string? Website { get; set; }
        public string? IndaVideo { get; set; }
        public string? Videa { get; set; }
    }
}
