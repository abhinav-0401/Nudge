namespace Nudge.Client.Dtos;

public record CreateRequestDto(
    string Url,
    string Method,
    string? Body
);