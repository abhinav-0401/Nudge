namespace Nudge.Client.Models;

public record Request
(
    int RequestId,
    string Url,
    string Method,
    string? Body
);