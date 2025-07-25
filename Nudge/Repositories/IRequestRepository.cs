using Nudge.Lib.Models;
using Nudge.Lib.Dtos;

namespace Nudge.Repositories;

public interface IRequestRepository
{
    public Task<bool> CreateRequestAsync(CreateRequestDto createRequestDto);
    public Task UpdateRequestAsync(UpdateRequestDto updateRequestDto);
    public Task<int?> DeleteRequestAsync(int id);
    public Task<Request?> GetRequestAsync(int id);
    public Task<List<Request>?> GetAllRequestsAsync();
}