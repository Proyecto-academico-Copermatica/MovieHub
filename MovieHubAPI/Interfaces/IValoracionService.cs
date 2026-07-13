using MovieHubAPI.DTOs.Valoracion;

namespace MovieHubAPI.Interfaces;

public interface IValoracionService
{
    Task<List<ValoracionDto>> GetByPeliculaIdAsync(int peliculaId);
    Task<ValoracionDto> CreateAsync(CreateValoracionDto dto, long usuarioId);
    Task<ValoracionDto?> UpdateAsync(int id, int puntuacion, long usuarioId);
    Task<bool> DeleteAsync(int id, long usuarioId);
}
