using Mapster;
using Microsoft.EntityFrameworkCore;
using MovieHubAPI.Data;
using MovieHubAPI.DTOs;
using MovieHubAPI.DTOs.Pelicula;
using MovieHubAPI.DTOs.Valoraciones;
using MovieHubAPI.Interfaces;
using MovieHubAPI.Models;


namespace MovieHubAPI.Services
{
    public class PeliculaService : IPeliculaService
    {
        private readonly MovieHubDbContext _context;

        public PeliculaService(MovieHubDbContext context) => _context = context;

        public async Task<PaginadosDto<PeliculaDto>> GetAllPaginadoAsync(int page, int pageSize)
        {
            var query = _context.Peliculas
                .Include(p => p.PeliculaGeneros).ThenInclude(pg => pg.Genero)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var peliculas = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginadosDto<PeliculaDto>(
                peliculas.Adapt<List<PeliculaDto>>(),
                page,
                pageSize,
                totalCount,
                (int)Math.Ceiling(totalCount / (double)pageSize)
            );
        }

        public async Task<PeliculaDto?> GetByIdAsync(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.PeliculaGeneros).ThenInclude(pg => pg.Genero)
                .FirstOrDefaultAsync(p => p.Id == id);
            return pelicula?.Adapt<PeliculaDto>();
        }

        public async Task<PeliculaDto> CreateAsync(CreatePeliculaDto dto)
        {
            var pelicula = new PeliculaModel
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Duracion = dto.Duracion,
                Anio = dto.AnioEstreno,
                Director = dto.Director,
                PosterUrl = dto.Imagen,
                PeliculaGeneros = dto.GeneroIds
                    .Select(gId => new PeliculaGeneroModel { GeneroId = gId })
                    .ToList()
            };

            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();

            await _context.Entry(pelicula)
                .Collection(p => p.PeliculaGeneros).Query()
                .Include(pg => pg.Genero).LoadAsync();

            return pelicula.Adapt<PeliculaDto>();
        }

        public async Task<PeliculaDto?> UpdateAsync(int id, UpdatePeliculaDto dto)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.PeliculaGeneros)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (pelicula is null) return null;

            pelicula.Titulo = dto.Titulo;
            pelicula.Descripcion = dto.Descripcion;
            pelicula.Duracion = dto.Duracion;
            pelicula.Anio = dto.AnioEstreno;
            pelicula.Director = dto.Director;
            pelicula.PosterUrl = dto.Imagen;

            _context.PeliculaGeneros.RemoveRange(pelicula.PeliculaGeneros);
            pelicula.PeliculaGeneros = dto.GeneroIds
                .Select(gId => new PeliculaGeneroModel { PeliculaId = id, GeneroId = gId })
                .ToList();

            await _context.SaveChangesAsync();

            await _context.Entry(pelicula)
                .Collection(p => p.PeliculaGeneros).Query()
                .Include(pg => pg.Genero).LoadAsync();

            return pelicula.Adapt<PeliculaDto>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula is null) return false;

            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return true;
        }
         public async Task RecalcularYGuardarValoracionAsync(long usuarioId, int peliculaId, double puntuacion)
        {

            // 1. Validar si la película existe
            var pelicula = await _context.Peliculas.FindAsync(peliculaId);
            if (pelicula == null)
            {
                throw new KeyNotFoundException($"La película con ID {peliculaId} no existe.");
            }

            // 2. Validar si el usuario existe (Ajusta '_context.Usuarios' al nombre real de tu tabla de usuarios)
            // var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
            // if (!usuarioExiste)
            // {
            //     throw new KeyNotFoundException($"El usuario con ID {usuarioId} no existe.");
            // }

            // 1. Verificar si ya existe una valoración de este usuario para esta película
            var valoracionExistente = await _context.Valoraciones
        .FirstOrDefaultAsync(v => v.PeliculaId == peliculaId && v.UsuarioId == usuarioId);

        if (valoracionExistente == null)
        {
            var nuevaValoracion = new ValoracionModel
            {
                UsuarioId = usuarioId,
                PeliculaId = peliculaId,
                Puntuacion = puntuacion,
                Fecha = DateTime.UtcNow
                // Deja que Entity Framework maneje las propiedades de navegación automáticamente por los IDs
            };
            _context.Valoraciones.Add(nuevaValoracion);
        }
        else
        {
            valoracionExistente.Puntuacion = puntuacion;
            valoracionExistente.Fecha = DateTime.UtcNow;
            _context.Valoraciones.Update(valoracionExistente);
        }
            // Guardar cambios iniciales para que el promedio incluya el nuevo dato
            await _context.SaveChangesAsync();

            //  Calcular el promedio actual de la película
            var promedio = await _context.Valoraciones
            .Where(v => v.PeliculaId == peliculaId)
            .AverageAsync(v => (double?)v.Puntuacion) ?? 0.0;

            // 5. Actualizar la película (ya la buscamos arriba, está en memoria)
            pelicula.PuntuacionMedia = Math.Round(promedio, 2);
            _context.Peliculas.Update(pelicula);
            
            await _context.SaveChangesAsync();
        }
         public async Task<List<GuardarValoracionDto>> ListarValoracionesPeliculaAsync(int peliculaId)
        {
            var valoraciones = await _context.Valoraciones
                .Where(v => v.PeliculaId == peliculaId)
                .ToListAsync();

            return valoraciones.Adapt<List<GuardarValoracionDto>>();
        }
    }
}
