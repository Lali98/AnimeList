using AnimeList.Data;
using AnimeList.Dtos.Anime;
using AnimeList.Models;
using AnimeList.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Service
{
    public class AnimeService(AppDbContext db) : IAnimeService
    {
        private readonly AppDbContext _db = db;

        public async Task<List<AnimeResponseDto>> GetSeasonAsync(
            int year,
            AnimeSeason season)
        {
            return await _db.Animes
                .AsNoTracking()
                .Where(x =>
                    x.Year == year &&
                    x.Season == season)
                .Select(x => new AnimeResponseDto
                {
                    Id = x.Id,
                    MalId = x.MalId,
                    Titles = x.Titles,
                    Descriptions = x.Descriptions,
                    Type = x.Type,
                    TrailerUrls = x.TrailerUrls,
                    ImageUrl = x.ImageUrl,
                    Year = x.Year,
                    Season = x.Season,
                    Episodes = x.Episodes,
                    Duration = x.Duration,
                    MalScore = x.MalScore,

                    Genres = x.Genres
                        .Select(g => new GenreResponseDto
                        {
                            Id = g.Id,
                            Name = g.Name
                        })
                        .ToList(),

                    Studios = x.Studios
                        .Select(s => new StudioResponseDto
                        {
                            Id = s.Id,
                            Name = s.Name
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<AnimeResponseDto?> GetAnimeByIdAsync(int id) => await _db.Animes
                .AsNoTracking()
                .Where(
                x => x.Id == id)
                .Select(x => new AnimeResponseDto
                {
                    Id = x.Id,
                    MalId = x.MalId,
                    Titles = x.Titles,
                    Descriptions = x.Descriptions,
                    Type = x.Type,
                    TrailerUrls = x.TrailerUrls,
                    ImageUrl = x.ImageUrl,
                    Year = x.Year,
                    Season = x.Season,
                    Episodes = x.Episodes,
                    Duration = x.Duration,
                    MalScore = x.MalScore,
                    Genres = x.Genres
                        .Select(g => new GenreResponseDto
                        {
                            Id = g.Id,
                            Name = g.Name
                        })
                        .ToList(),
                    Studios = x.Studios
                        .Select(s => new StudioResponseDto
                        {
                            Id = s.Id,
                            Name = s.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

        public async Task<AnimeResponseDto?> GetAnimeByMalIdAsync(int malId) => await _db.Animes
                .AsNoTracking()
                .Where(x => x.MalId == malId)
                .Select(x => new AnimeResponseDto
                {
                    Id = x.Id,
                    MalId = x.MalId,
                    Titles = x.Titles,
                    Descriptions = x.Descriptions,
                    Type = x.Type,
                    TrailerUrls = x.TrailerUrls,
                    ImageUrl = x.ImageUrl,
                    Year = x.Year,
                    Season = x.Season,
                    Episodes = x.Episodes,
                    Duration = x.Duration,
                    MalScore = x.MalScore,
                    Genres = x.Genres
                        .Select(g => new GenreResponseDto
                        {
                            Id = g.Id,
                            Name = g.Name
                        })
                        .ToList(),
                    Studios = x.Studios
                        .Select(s => new StudioResponseDto
                        {
                            Id = s.Id,
                            Name = s.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
    }
}