using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHubAPI.Data; 
using MovieHubAPI.DTOs.Favoritos;
using MovieHubAPI.Models;

namespace MovieHubAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritosController(MovieHubDbContext context) : ControllerBase
{
    private readonly MovieHubDbContext _context = context;

    // POST: api/favoritos
    [HttpPost]
        public async Task<IActionResult> AnadirFavorito([FromBody] GuardarFavoritoDto dto)
        {
            var existe = await _context.Favoritos
                .AnyAsync(f => f.UsuarioId == dto.UsuarioId && f.PeliculaId == dto.PeliculaId);

            if (existe) 
                return BadRequest("La película ya se encuentra en tus favoritos.");

            var favorito = new FavoritoModel
            {
                UsuarioId = dto.UsuarioId,
                PeliculaId = dto.PeliculaId
            };
             _context.Favoritos.Add(favorito);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Película añadida a favoritos con éxito." });
        }

        // DELETE: api/favoritos/usuario/123/pelicula/5
        [HttpDelete("usuario/{usuarioId}/pelicula/{peliculaId}")]
        public async Task<IActionResult> EliminarFavorito(long usuarioId, int peliculaId)
        {
            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.PeliculaId == peliculaId);

            if (favorito == null) 
                return NotFound("El elemento no existe en favoritos.");

            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Película eliminada de favoritos con éxito." });
    }
    // GET: api/favoritos/usuario/123
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<List<PeliculaModel>>> ListarFavoritosUsuario(long usuarioId)
        {
            var peliculasFavoritas = await _context.Favoritos
                .Where(f => f.UsuarioId == usuarioId)
                .Include(f => f.Pelicula) 
                .Select(f => f.Pelicula)
                .ToListAsync();

            return Ok(peliculasFavoritas);
        }

}
