namespace Nudge.Lib.Dtos;

public record ExecuteRequestDto(
    string Url,
    string Method,
    string? Body
);