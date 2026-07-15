using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHubAPI.DTOs.Valoraciones;
using MovieHubAPI.Models;
using MovieHubAPI.Interfaces;


namespace MovieHubAPI.Controllers;


    [ApiController]
    [Route("api/[controller]")]
    public class ValoracionesController(IPeliculaService peliculaService) : ControllerBase
    {
        private readonly IPeliculaService _peliculaService = peliculaService;

    // POST: api/valoraciones
    [HttpPost]
        public async Task<IActionResult> CrearOActualizarValoracion([FromBody] GuardarValoracionDto dto)
        {
            if (dto.Puntuacion < 1 || dto.Puntuacion > 5)
                return BadRequest("La puntuación debe estar entre 1 y 5.");

            try
            {
                await _peliculaService.RecalcularYGuardarValoracionAsync(dto.UsuarioId, dto.PeliculaId, dto.Puntuacion);
                return Ok(new { mensaje = "Valoración guardada y puntuación media actualizada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        // GET: api/valoraciones/pelicula/5
        [HttpGet("pelicula/{peliculaId}")]
        public async Task<ActionResult<List<ValoracionModel>>> ListarValoraciones(int peliculaId)
        {
            var lista = await _peliculaService.ListarValoracionesPeliculaAsync(peliculaId);
            return Ok(lista);
        }

}
