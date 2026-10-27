using Microsoft.AspNetCore.Mvc;
using ValeraApi.DTOs;
using ValeraApi.Services;

namespace ValeraApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValeraController : ControllerBase
{
    private readonly IValeraService _service;

    public ValeraController(IValeraService service)
    {
        _service = service;
    }

    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ValeraDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ValeraDto>> GetById(int id)
    {
        var valera = await _service.GetByIdAsync(id);
        return valera is null ? NotFound() : Ok(valera);
    }

    
    [HttpPost]
    public async Task<ActionResult<ValeraDto>> Create([FromBody] CreateValeraDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ValeraDto>> Update(int id, [FromBody] UpdateValeraDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) ? NoContent() : NotFound();

    
    [HttpPost("{id:int}/work")]
    public async Task<ActionResult<ValeraActionResultDto>> Work(int id) =>
        ToResponse(await _service.GoToWorkAsync(id));

    
    [HttpPost("{id:int}/contemplate")]
    public async Task<ActionResult<ValeraActionResultDto>> Contemplate(int id) =>
        ToResponse(await _service.ContemplateNatureAsync(id));

    
    [HttpPost("{id:int}/wine")]
    public async Task<ActionResult<ValeraActionResultDto>> Wine(int id) =>
        ToResponse(await _service.DrinkWineWatchSeriesAsync(id));


    [HttpPost("{id:int}/bar")]
    public async Task<ActionResult<ValeraActionResultDto>> Bar(int id) =>
        ToResponse(await _service.GoToBarAsync(id));

    
    [HttpPost("{id:int}/marginals")]
    public async Task<ActionResult<ValeraActionResultDto>> Marginals(int id) =>
        ToResponse(await _service.DrinkWithMarginalsAsync(id));


    [HttpPost("{id:int}/sing")]
    public async Task<ActionResult<ValeraActionResultDto>> Sing(int id) =>
        ToResponse(await _service.SingInMetroAsync(id));

    
    [HttpPost("{id:int}/sleep")]
    public async Task<ActionResult<ValeraActionResultDto>> Sleep(int id) =>
        ToResponse(await _service.SleepAsync(id));

    
    private ActionResult<ValeraActionResultDto> ToResponse(ValeraActionResultDto? result)
    {
        if (result is null)
            return NotFound();

        return result.Success ? Ok(result) : BadRequest(result);
    }
}