using AnimeList.Dtos.Genre;
using AnimeList.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController (IGenreService service) : ControllerBase
    {
        private readonly IGenreService _service = service;

        [HttpGet]
        public async Task<ActionResult<List<GenreDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenreDto>> GetById(int id)
        {
            var genre = await _service.GetByIdAsync(id);

            if (genre is null)
                return NotFound(new
                {
                    status = StatusCodes.Status404NotFound,
                    message = $"Nem található műfaj ezen az id-n: {id}"
                });

            return Ok(genre);
        }
    }
}
