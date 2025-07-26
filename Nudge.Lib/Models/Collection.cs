namespace Nudge.Lib.Models;

public record Collection(
    int CollectionId,
    string Name
)
{
    public List<Request>? Requests { get; set; }
};