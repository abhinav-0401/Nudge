namespace Nudge.Dtos;

public record UpdateRequestDto(
    string Url,
    string Method,
    string? Body
);