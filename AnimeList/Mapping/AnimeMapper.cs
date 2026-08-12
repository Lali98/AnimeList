using AnimeList.Dtos.MyAnimeList;
using AnimeList.Models;

namespace AnimeList.Mapping
{
    public class AnimeMapper
    {
        public Anime ToEntity(MALAnimeDto dto)
        {
            return new Anime
            {
                MalId = dto.Id,
                Titles = new Title
                {
                    JpRomaji = dto.Title,
                    JpKanji = dto.AlternativeTitles?.Japanese ?? string.Empty,
                    En = dto.AlternativeTitles?.English ?? string.Empty,
                },
                Descriptions = new Description
                {
                    En = dto.Synopsis ?? string.Empty,
                    Hu = null,
                },
                Type = ParseType(dto.MediaType),
                ImageUrl = dto.Picture?.Medium,
                TrailerUrls = null,
                Year = dto.StartSeason?.Year,
                Season = ParseSeason(dto.StartSeason?.Season),
                Episodes = dto.NumEpisodes,
                Duration = dto.AverageEpisodeDuration / 60,
                MalScore = dto.Mean ?? 0,
            };
        }
        private static AnimeType ParseType(string? type)
        {
            if (Enum.TryParse<AnimeType>(type, true, out var result))
                return result;
            return AnimeType.Unknown;
        }

        private static AnimeSeason? ParseSeason(string? season)
        {
            if (Enum.TryParse<AnimeSeason>(season, true, out var result))
                return result;
            return null;
        }
    }
}
