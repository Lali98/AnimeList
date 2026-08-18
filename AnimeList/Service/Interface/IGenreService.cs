using AnimeList.Dtos.Genre;

namespace AnimeList.Service.Interface
{
    public interface IGenreService
    {
        Task<List<GenreDto>> GetAllAsync();
        Task<GenreDto?> GetByIdAsync(int id);
    }
}
