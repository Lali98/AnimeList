using AnimeList.Data;
using AnimeList.Dtos.Genre;
using AnimeList.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Service
{
    public class GenreService (AppDbContext db) : IGenreService
    {
        private readonly AppDbContext _db = db;

        public async Task<List<GenreDto>> GetAllAsync()
        {
            return await _db.Genres
                .AsNoTracking()
                .Select(x => new GenreDto
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .ToListAsync();
        }

        public async Task<GenreDto?> GetByIdAsync(int id)
        {
            return await _db.Genres
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new GenreDto
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .FirstOrDefaultAsync();
        }
    }
}
