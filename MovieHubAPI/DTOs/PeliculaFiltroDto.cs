namespace MovieHubAPI.DTOs;

public record PeliculaFiltroDto(
    string? Titulo,
    int? GeneroId,
    string? Orden
);
