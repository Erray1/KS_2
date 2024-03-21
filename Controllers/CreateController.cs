using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Task2.DataContext;
using Task2.HttpModels;

namespace Task2.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CreateController : ControllerBase
{
    private readonly DataCreationService _createService;
    public CreateController(DataCreationService createService)
    {
        _createService = createService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest request)
    {
        var errors = await _createService.CreateEntitiesAsync(request);
        if (errors.Count() != 0)
        {
            return BadRequest(errors);
        }
        return Created();
    }

    [HttpPatch("delete-all")]
    public async Task<IActionResult> Delete()
    {
        await _createService.DeleteAllAsync();
        return Ok();
    }

    [HttpPost("create-from-memory")]
    public async Task<IActionResult> CreateFromMemory()
    {
        using (FileStream stream = new("DataContext/data.json", FileMode.Open, FileAccess.Read))
        {
            var data = await JsonSerializer.DeserializeAsync<CreateRequest>(stream);
            var errors = await _createService.CreateEntitiesAsync(data!);
            if (errors.Count() != 0)
            {
                return BadRequest(errors);
            }
            return Created();
        }

    }
}
