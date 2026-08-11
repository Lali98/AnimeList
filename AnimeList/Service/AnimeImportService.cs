using AnimeList.Data;
using AnimeList.Dtos.MyAnimeList;
using AnimeList.Mapping;
using AnimeList.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Service
{
    public class AnimeImportService
    {
        private readonly MalApiService _mal;
        private readonly AnimeMapper _mapper;
        private readonly AppDbContext _db;

        public AnimeImportService(MalApiService mal, AnimeMapper mapper, AppDbContext db)
        {
            _mal = mal;
            _mapper = mapper;
            _db = db;
        }

        public async Task ImportSeasonAsync(int year, AnimeSeason season)
        {
            Console.WriteLine("Import inditása...");

            var response = await _mal.GetSeasonAsync(year, season);
            foreach (var dto in response.Data)
            {
                var anime = _mapper.ToEntity(dto.Node);
                if (anime.Year != year)
                    continue;
                anime.Genres = await GetGenresAsync(dto.Node.Genres);
                anime.Studios = await GetStudiosAsync(dto.Node.Studios);
                await SaveAnimeAsync(anime);
            }

            Console.WriteLine("Import befejezve.");
        }

        private async Task SaveAnimeAsync(Anime anime)
        {
            var existingAnime = await _db.Animes
                .Include(x => x.Genres)
                .Include(x => x.Studios)
                .FirstOrDefaultAsync(x => x.MalId == anime.MalId);

            if (existingAnime is null)
            {
                _db.Animes.Add(anime);
                await _db.SaveChangesAsync();

                Console.WriteLine($"  + Új anime: {anime.Titles.JpRomaji}");
                return;
            }

            existingAnime.Titles = anime.Titles;
            existingAnime.Descriptions = anime.Descriptions;
            existingAnime.Type = anime.Type;
            existingAnime.TrailerUrls = anime.TrailerUrls;
            existingAnime.ImageUrl = anime.ImageUrl;
            existingAnime.Year = anime.Year;
            existingAnime.Season = anime.Season;
            existingAnime.Episodes = anime.Episodes;
            existingAnime.Duration = anime.Duration;
            existingAnime.MalScore = anime.MalScore;

            existingAnime.Genres.Clear();

            foreach (var genre in anime.Genres)
            {
                existingAnime.Genres.Add(genre);
            }

            existingAnime.Studios.Clear();

            foreach (var studio in anime.Studios)
            {
                existingAnime.Studios.Add(studio);
            }

            await _db.SaveChangesAsync();

            Console.WriteLine($"  ~ Frissítve: {anime.Titles.JpRomaji}");
        }

        private async Task<List<Genre>> GetGenresAsync(IEnumerable<MALGenreDto> genres)
        {
            var result = new List<Genre>();
            foreach (var genre in genres)
            {
                var existing = await _db.Genres
                    .FirstOrDefaultAsync(x => x.Name == genre.Name);

                if (existing is null)
                {
                    existing = new Genre
                    {
                        Name = genre.Name,
                    };
                    _db.Genres.Add(existing);
                }
                result.Add(existing);
            }
            return result;
        }

        private async Task<List<Studio>> GetStudiosAsync(IEnumerable<MALStudioDto> studios)
        {
            var result = new List<Studio>();
            foreach (var studio in studios)
            {
                var existing = await _db.Studios
                    .FirstOrDefaultAsync(x => x.Name == studio.Name);

                if (existing is null)
                {
                    existing = new Studio
                    {
                        Name = studio.Name,
                    };
                    _db.Studios.Add(existing);
                }
                result.Add(existing);
            }
            return result;
        }
    }
}
