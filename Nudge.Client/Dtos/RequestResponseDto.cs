namespace Nudge.Client.Dtos;

public record RequestResponseDto(
    int StatusCode,
    string? Content,
    Dictionary<string, string> Headers
);