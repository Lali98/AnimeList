using AnimeList.Dtos.FanSub;
using AnimeList.Models;
using AnimeList.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FanSubController(IFanSubService service) : ControllerBase
    {
        private readonly IFanSubService _service = service;

        [HttpGet]
        public async Task<ActionResult<List<FanSubDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FanSubDto>> GetById(int id)
        {
            var fanSub = await _service.GetByIdAsync(id);

            if (fanSub is null)
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = $"Nem található fansub ezen az id-n: {id}"
                });

            return Ok(fanSub);
        }

        [HttpPost]
        public async Task<ActionResult<FanSubDto>> Create(CreateFanSubDto dto)
        {
            var fanSub = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new {id = fanSub.Id},
                fanSub);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<FanSubDto>> Update(int id, UpdateFanSubDto dto)
        {
            var fanSub = await _service.UpdateAsync(id, dto);

            if (fanSub is null)
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = $"Nem található fansub ezen a id-n: {id}"
                });

            return Ok(fanSub);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if(!deleted)
            {
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = $"Nem található fansub ezen a id-n: {id}"
                });
            }
            return NoContent();
        }

        [HttpPost("{fanSubId:int}/anime/{animeId:int}")]
        public async Task<IActionResult> AddAnime(int fanSubId, int animeId)
        {
            var result = await _service.AddAnimeAsync(fanSubId, animeId);

            if (!result)
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = $"Nem található fansub vagy anime ezen a id-n: FanSub: {fanSubId}, Anime: {animeId}"
                });

            return NoContent();
        }

        [HttpDelete("{fanSubId:int}/anime/{animeId}")]
        public async Task<IActionResult> RemoveAnime(int fanSubId, int animeId)
        {
            var result = await _service.RemoveAnimeAsync(fanSubId, animeId);

            if (!result)
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = $"Nem található fansub vagy anime ezen a id-n: FanSub: {fanSubId}, Anime: {animeId}"
                });
            return NoContent();
        }
    }
}
