namespace Nudge.Client.Dtos;

public record ExecuteRequestDto(
    string Url,
    string Method,
    string? Body
);