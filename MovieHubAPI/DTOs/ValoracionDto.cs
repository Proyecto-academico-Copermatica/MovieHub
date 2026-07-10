namespace MovieHubAPI.DTOs;

public record ValoracionDto(
    int Id,
    string UsuarioEmail,
    int PeliculaId,
    int Puntuacion,
    DateTime Fecha
);
