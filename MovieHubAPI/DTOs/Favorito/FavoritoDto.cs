namespace MovieHubAPI.DTOs.Favorito;

public record FavoritoDto(
    int PeliculaId,
    string Titulo,
    string? Imagen,
    double PuntuacionMedia
);
