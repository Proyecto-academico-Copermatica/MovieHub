using Microsoft.EntityFrameworkCore;
using MovieHubAPI.DTOs.Valoracion;
using MovieHubAPI.Interfaces;

namespace MovieHubAPI.Services;

public class ValoracionService : IValoracionService
{
    private readonly MovieHubDbContext _context;

    public ValoracionService(MovieHubDbContext context) => _context = context;

    public async Task<List<ValoracionDto>> GetByPeliculaIdAsync(int peliculaId)
    {
        return await _context.Valoraciones
            .Where(v => v.PeliculaId == peliculaId)
            .Include(v => v.Usuario)
            .OrderByDescending(v => v.Fecha)
            .Select(v => new ValoracionDto(
                v.Id,
                v.Usuario.Email!,
                v.Puntuacion,
                v.Fecha
            ))
            .ToListAsync();
    }

    public async Task<ValoracionDto> CreateAsync(CreateValoracionDto dto, long usuarioId)
    {
        var valoracion = new ValoracionModel
        {
            UsuarioId = usuarioId,
            PeliculaId = dto.PeliculaId,
            Puntuacion = dto.Puntuacion,
            Fecha = DateTime.UtcNow
        };

        _context.Valoraciones.Add(valoracion);
        await _context.SaveChangesAsync();
        await RecalcularPuntuacionMediaAsync(dto.PeliculaId);

        return new ValoracionDto(
            valoracion.Id,
            (await _context.Users.FindAsync(usuarioId))!.Email!,
            valoracion.Puntuacion,
            valoracion.Fecha
        );
    }

    public async Task<ValoracionDto?> UpdateAsync(int id, int puntuacion, long usuarioId)
    {
        var valoracion = await _context.Valoraciones
            .Include(v => v.Usuario)
            .FirstOrDefaultAsync(v => v.Id == id && v.UsuarioId == usuarioId);

        if (valoracion is null) return null;

        valoracion.Puntuacion = puntuacion;
        valoracion.Fecha = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await RecalcularPuntuacionMediaAsync(valoracion.PeliculaId);

        return new ValoracionDto(
            valoracion.Id,
            valoracion.Usuario.Email!,
            valoracion.Puntuacion,
            valoracion.Fecha
        );
    }

    public async Task<bool> DeleteAsync(int id, long usuarioId)
    {
        var valoracion = await _context.Valoraciones
            .FirstOrDefaultAsync(v => v.Id == id && v.UsuarioId == usuarioId);

        if (valoracion is null) return false;

        var peliculaId = valoracion.PeliculaId;
        _context.Valoraciones.Remove(valoracion);
        await _context.SaveChangesAsync();
        await RecalcularPuntuacionMediaAsync(peliculaId);

        return true;
    }

    private async Task RecalcularPuntuacionMediaAsync(int peliculaId)
    {
        var media = await _context.Valoraciones
            .Where(v => v.PeliculaId == peliculaId)
            .AverageAsync(v => (double?)v.Puntuacion) ?? 0;

        var pelicula = await _context.Peliculas.FindAsync(peliculaId);
        if (pelicula is not null)
        {
            pelicula.PuntuacionMedia = Math.Round(media, 1);
            await _context.SaveChangesAsync();
        }
    }
}
