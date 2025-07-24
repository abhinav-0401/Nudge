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
    private IHttpClientFactory _httpClientFactory;

    public RequestManagerController(
        IRequestRepository requestRepository,
        IHttpClientFactory httpClientFactory
    )
    {
        _requestRepository = requestRepository;
        _httpClientFactory = httpClientFactory;
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

    [HttpPost("execute")]
    public async Task<ActionResult<RequestResponseDto>> ExecuteRequest([FromBody] ExecuteRequestDto executeRequestDto)
    {
        var httpClient = _httpClientFactory.CreateClient("NudgeExecutorClient");

        using var requestMsg = new HttpRequestMessage();
        requestMsg.RequestUri = new Uri(executeRequestDto.Url);
        requestMsg.Method = new HttpMethod(executeRequestDto.Method);
        requestMsg.Content = JsonContent.Create(executeRequestDto.Body);

        using var response = await httpClient.SendAsync(requestMsg);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var execResponse = new RequestResponseDto(
            StatusCode: (int)response.StatusCode,
            Content: await response.Content.ReadAsStringAsync(),
            Headers: response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))
        );

        return Ok(execResponse);
    }
}