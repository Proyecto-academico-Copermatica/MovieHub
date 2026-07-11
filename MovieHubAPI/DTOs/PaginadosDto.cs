namespace MovieHubAPI.DTOs;

public record PaginadosDto<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);
