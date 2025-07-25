namespace Nudge.Lib.Dtos;

public record CreateRequestDto(
    string Url,
    string Method,
    string? Body
);