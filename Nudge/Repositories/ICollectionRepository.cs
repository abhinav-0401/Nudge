using Nudge.Lib.Models;
using Nudge.Lib.Dtos;

namespace Nudge.Repositories;

public interface ICollectionRepository
{
    public Task<bool> CreateCollectionAsync(CreateCollectionDto createCollectionDto);
    // public Task UpdateCollectionAsync(UpdateRequestDto updateRequestDto);
    public Task<int?> DeleteCollectionAsync(int id);
    public Task<Collection?> GetCollectionAsync(int id);
    public Task<List<Collection>?> GetAllCollectionsAsync();
}