namespace Nudge.Client.Models;

public record Request
(
    string Url,
    HttpMethod Method,
    string? Body
);