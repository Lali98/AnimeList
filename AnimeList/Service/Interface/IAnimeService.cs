using AnimeList.Dtos.Anime;
using AnimeList.Models;

namespace AnimeList.Service.Interface
{
    public interface IAnimeService
    {
        Task<List<AnimeResponseDto>> GetSeasonAsync(int year, AnimeSeason season);
        Task<AnimeResponseDto?> GetAnimeByIdAsync(int id);
        Task<AnimeResponseDto?> GetAnimeByMalIdAsync(int malId);
    }
}
