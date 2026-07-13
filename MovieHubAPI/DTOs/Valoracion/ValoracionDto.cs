namespace MovieHubAPI.DTOs.Valoracion;

public record ValoracionDto(
    int Id,
    string UsuarioEmail,
    int Puntuacion,
    DateTime Fecha
);
