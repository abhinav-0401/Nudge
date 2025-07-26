using Microsoft.AspNetCore.Mvc;
using Nudge.Lib.Dtos;
using Nudge.Repositories;

namespace Nudge.Controllers;

[Route("api/collection")]
[ApiController]
public class CollectionManagerController : ControllerBase
{
    private ICollectionRepository _collectionRepository;

    public CollectionManagerController(ICollectionRepository collectionRepository)
    {
        _collectionRepository = collectionRepository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Controller?>> GetCollection(int id)
    {
        var controller = await _collectionRepository.GetCollectionAsync(id);

        if (controller is null)
        {
            return NotFound();
        }

        return Ok(controller);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionDto createCollectionDto)
    {
        var isSuccessful = await _collectionRepository.CreateCollectionAsync(createCollectionDto);
        if (!isSuccessful)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        var deletedId = await _collectionRepository.DeleteCollectionAsync(id);
        if (deletedId is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        return Ok();
    }
}