using AnimeList.Models;

namespace AnimeList.Dtos.Anime
{
    public class AnimeResponseDto
    {
        public int Id { get; set; }
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

        public List<GenreResponseDto> Genres { get; set; } = [];
        public List<StudioResponseDto> Studios { get; set; } = [];
    }
}