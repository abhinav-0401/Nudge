namespace Nudge.Lib.Dtos;

public record UpdateRequestDto(
    string Url,
    string Method,
    string? Body
);