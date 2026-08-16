using AnimeList.Models;
using AnimeList.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController(IAnimeService animeService) : ControllerBase
    {
        private readonly IAnimeService _animeService = animeService;

        [HttpGet("season/{year:int}/{season}")]
        public async Task<ActionResult<List<Anime>>> GetSeason(int year, AnimeSeason season)
        {
            var animes = await _animeService.GetSeasonAsync(year, season);
            if (animes.Count == 0)
            {
                return NotFound(new
                {
                    status = 404,
                    message = $"Nem található anime a {year}-es {season} évszakban."
                });
            }
            return Ok(animes);
        }

        [HttpGet("id/{id:int}")]
        public async Task<ActionResult<Anime>> GetAnimeById(int id)
        {
            var anime = await _animeService.GetAnimeByIdAsync(id);
            if (anime is null)
            {
                return NotFound(new
                {
                    status = 404,
                    message = $"Nem található anime a megadott azonosítóval: {id}."
                });
            }
            return Ok(anime);
        }

        [HttpGet("mal/{malId:int}")]
        public async Task<ActionResult<Anime>> GetAnimeByMalId(int malId)
        {
            var anime = await _animeService.GetAnimeByMalIdAsync(malId);
            if (anime is null)
            {
                return NotFound(new
                {
                    status = 404,
                    message = $"Nem található anime a megadott MyAnimeList azonosítóval: {malId}."
                });
            }
            return Ok(anime);
        }
    }
}
