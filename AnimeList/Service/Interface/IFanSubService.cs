using AnimeList.Dtos.FanSub;

namespace AnimeList.Service.Interface
{
    public interface IFanSubService
    {
        Task<List<FanSubDto>> GetAllAsync();
        Task<FanSubDto?> GetByIdAsync(int id);

        Task<FanSubDto> CreateAsync(CreateFanSubDto dto);
        Task<FanSubDto?> UpdateAsync(int id, UpdateFanSubDto dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> AddAnimeAsync(int fanSubId, int animeId);
        Task<bool> RemoveAnimeAsync(int fanSubId, int animeId);
    }
}
