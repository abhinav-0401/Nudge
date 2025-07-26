namespace Nudge.Lib.Models;

public record Request(
    int RequestId,
    string Url,
    string Method,
    string? Body,
    int CollectionId
);