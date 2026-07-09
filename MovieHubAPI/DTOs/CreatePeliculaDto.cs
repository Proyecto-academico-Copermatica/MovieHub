namespace MovieHubAPI.DTOs;

public record CreatePeliculaDto(
    string Titulo,
    string Descripcion,
    int Duracion,
    int AnioEstreno,
    string Director,
    string? Imagen,
    List<int> GeneroIds
);
