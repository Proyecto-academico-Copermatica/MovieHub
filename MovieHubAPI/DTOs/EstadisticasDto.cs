namespace MovieHubAPI.DTOs;

public record EstadisticasDto(
    int TotalPeliculas,
    int TotalValoraciones,
    double PuntuacionMediaGlobal
);
