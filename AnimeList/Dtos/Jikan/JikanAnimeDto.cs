namespace AnimeList.Dtos.Jikan
{
    public class JikanAnimeDto
    {
        public int MalId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleEnglish { get; set; }
        public string? TitleJapanese { get; set; }
        public string? Synopsis { get; set; }
        public string? Type { get; set; }
        public string? TrailerUrl { get; set; }
        public string? ImageUrl { get; set; }
        public int? Year { get; set; }
        public string? Season { get; set; }
        public int? Episodes { get; set; }
        public string? Duration { get; set; }
        public float? Score { get; set; }
        public List<JikanGenreDto> Genres { get; set; } = [];
        public List<JikanStudioDto> Studios { get; set; } = [];
    }
}
