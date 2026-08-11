namespace AnimeList.Dtos.MyAnimeList
{
    public class MALSeasonResponseDto
    {
        public List<MALAnimeNodeDto> Data { get; set; } = [];
        public MALPaginationDto? Pagination { get; set; }
        public MALSeasonDto? Season { get; set; }
    }

    public class MALAnimeNodeDto
    {
        public MALAnimeDto Node { get; set; } = new();
    }

    public class MALPaginationDto
    {
        public string? Next { get; set; }
    }

    public class MALSeasonDto
    {
        public int Year { get; set; }
        public string Season { get; set; } = string.Empty;
    }
}
