namespace MovieHubAPI.DTOs;

public record RegisterDto(
    string Email,
    string Nombre,
    string Password
);
