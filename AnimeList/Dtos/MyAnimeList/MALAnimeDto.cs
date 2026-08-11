using System.Text.Json.Serialization;

namespace AnimeList.Dtos.MyAnimeList
{
    public class MALAnimeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("main_picture")]
        public MALMainPictureDto? Picture { get; set; }
        public string? Synopsis { get; set; }
        public List<MALGenreDto> Genres { get; set; } = [];
        public List<MALStudioDto> Studios { get; set; } = [];
        [JsonPropertyName("alternative_titles")]
        public MALAlternativeTitlesDto? AlternativeTitles { get; set; }
        public string? MediaType { get; set; }
        public MALStartSeasonDto? StartSeason { get; set; }
        public int? NumEpisodes { get; set; }
        public int? AverageEpisodeDuration { get; set; }
        public float? Mean { get; set; }
    }

    public class MALMainPictureDto
    {
        public string? Medium { get; set; }
        public string? Large { get; set; }
    }

    public class MALAlternativeTitlesDto
    {
        public List<string> Synonyms { get; set; } = [];
        [JsonPropertyName("en")]
        public string? English { get; set; }
        [JsonPropertyName("ja")]
        public string? Japanese { get; set; }
    }

    public class MALStartSeasonDto
    {
        public int Year { get; set; }
        public string Season { get; set; } = string.Empty;
    }
}
