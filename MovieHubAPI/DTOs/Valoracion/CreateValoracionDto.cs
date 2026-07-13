namespace MovieHubAPI.DTOs.Valoracion;

public record CreateValoracionDto(
    int PeliculaId,
    int Puntuacion
);
