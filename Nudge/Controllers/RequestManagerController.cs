using Microsoft.AspNetCore.Mvc;
using Nudge.Models;
using Nudge.Dtos;
using Nudge.Repositories;

namespace Nudge.Controllers;

[Route("api/request")]
[ApiController]
public class RequestManagerController : ControllerBase
{
    private IRequestRepository _requestRepository;

    public RequestManagerController(
        IRequestRepository requestRepository
    )
    {
        _requestRepository = requestRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Request>>> GetAllRequests()
    {
        var requestList = await _requestRepository.GetAllRequestsAsync();
        if (requestList is null)
        {
            return NotFound();
        }
        return requestList;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto createRequestDto)
    {
        var isSuccessful = await _requestRepository.CreateRequestAsync(createRequestDto);
        if (isSuccessful)
        {
            return Ok();
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int?>> DeleteRequest(int id)
    {
        var deletedId = await _requestRepository.DeleteRequestAsync(id);
        return deletedId;
    }
}