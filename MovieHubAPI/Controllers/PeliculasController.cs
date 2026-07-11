using Microsoft.AspNetCore.Mvc;
using MovieHubAPI.ApiDefinitions;
using MovieHubAPI.DTOs;
using MovieHubAPI.DTOs.Pelicula;
using MovieHubAPI.Interfaces;

namespace MovieHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EndpointGroupName("Películas")]
    public class PeliculasController : ControllerBase, IPeliculaApi
    {
        private readonly IPeliculaService _peliculaService;

        public PeliculasController(IPeliculaService peliculaService)
        {
            _peliculaService = peliculaService;
        }

        public async Task<ActionResult<PaginadosDto<PeliculaDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageSize > 50) pageSize = 50;
            if (page < 1) page = 1;
            var resultado = await _peliculaService.GetAllPaginadoAsync(page, pageSize);
            return Ok(resultado);
        }

        public async Task<ActionResult<PeliculaDto>> GetById(int id)
        {
            var pelicula = await _peliculaService.GetByIdAsync(id);
            if (pelicula is null) return NotFound();
            return Ok(pelicula);
        }

        public async Task<ActionResult<PeliculaDto>> Create(CreatePeliculaDto dto)
        {
            var pelicula = await _peliculaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = pelicula.Id }, pelicula);
        }

        public async Task<ActionResult<PeliculaDto>> Update(int id, UpdatePeliculaDto dto)
        {
            var pelicula = await _peliculaService.UpdateAsync(id, dto);
            if (pelicula is null) return NotFound();
            return Ok(pelicula);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _peliculaService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
