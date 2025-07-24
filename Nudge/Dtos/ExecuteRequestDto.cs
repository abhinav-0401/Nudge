namespace Nudge;

public record ExecuteRequestDto(
    string Url,
    string Method,
    string? Body
);