using AnimeList.Data;
using AnimeList.Dtos.FanSub;
using AnimeList.Models;
using AnimeList.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Service
{
    public class FanSubService(AppDbContext db) : IFanSubService
    {
        private readonly AppDbContext _db = db;

        public async Task<List<FanSubDto>> GetAllAsync()
        {
            return await _db.FanSubs
                .AsNoTracking()
                .Select(x => new FanSubDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Links = new LinkDto
                    {
                        YouTube = x.Links.Youtube,
                        Discord = x.Links.Discord,
                        Website = x.Links.WebSite,
                        IndaVideo = x.Links.IndaVideo,
                        Videa = x.Links.Videa,
                    },
                    AnimeIds = x.Animes
                        .Select(a => a.Id)
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<FanSubDto?> GetByIdAsync(int id)
        {
            return await _db.FanSubs
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new FanSubDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Links = new LinkDto
                    {
                        YouTube = x.Links.Youtube,
                        Discord = x.Links.Discord,
                        Website = x.Links.WebSite,
                        IndaVideo = x.Links.IndaVideo,
                        Videa = x.Links.Videa,
                    },
                    AnimeIds = x.Animes
                        .Select(a => a.Id)
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<FanSubDto> CreateAsync(CreateFanSubDto dto)
        {
            var fanSub = new FanSub
            {
                Name = dto.Name,
                Description = dto.Description,
                Links = new Link
                {
                    Youtube = dto.Links.YouTube,
                    Discord = dto.Links.Discord,
                    WebSite = dto.Links.Website,
                    IndaVideo = dto.Links.IndaVideo,
                    Videa = dto.Links.Videa,
                }
            };

            _db.FanSubs.Add(fanSub);
            await _db.SaveChangesAsync();

            return (await GetByIdAsync(fanSub.Id))!;
        }

        public async Task<FanSubDto?> UpdateAsync(int id, UpdateFanSubDto dto)
        {
            var fanSub = await _db.FanSubs.FirstOrDefaultAsync(a => a.Id == id);
            
            if(fanSub is null)
                return null;

            fanSub.Name = dto.Name;
            fanSub.Description = dto.Description;
            fanSub.Links = new Link
            {
                Youtube = dto.Links.YouTube,
                Discord = dto.Links.Discord,
                WebSite = dto.Links.Website,
                IndaVideo = dto.Links.IndaVideo,
                Videa = dto.Links.Videa,
            };

            await _db.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var fanSub = await _db.FanSubs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (fanSub is null)
                return false;

            _db.FanSubs.Remove(fanSub);
            await _db.SaveChangesAsync();

            return true;
        } 

        public async Task<bool> AddAnimeAsync(int fanSubId, int animeId)
        {
            var fanSub = await _db.FanSubs
                .Include(x => x.Animes)
                .FirstOrDefaultAsync(x => x.Id == fanSubId);

            if (fanSub is null)
                return false;

            var anime = await _db.Animes.FirstOrDefaultAsync(x => x.Id == animeId);

            if (anime is null)
                return false;

            if (!fanSub.Animes.Any(x => x.Id == animeId))
            {
                fanSub.Animes.Add(anime);
                await _db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveAnimeAsync(int fanSubId, int animeId)
        {
            var fanSub = await _db.FanSubs
                .Include(x => x.Animes)
                .FirstOrDefaultAsync(x => x.Id == fanSubId);

            if (fanSub is null)
                return false;

            var anime = fanSub.Animes.FirstOrDefault(x => x.Id == animeId);

            if (anime is null)
                return false;

            fanSub.Animes.Remove(anime);

            await _db.SaveChangesAsync();

            return true;
        }
    }
}
