using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Group;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace LinkyAppBackend.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class Group(IGroupService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResults<GetGroupDto>>> Get([FromQuery] SieveModel query)
    {
        return await service.GetAll(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetGroupDto>> Get(string id)
    {
        return await service.GetById(id);
    }

    [HttpPost]
    public async Task<ActionResult<GetGroupDto>> Post([FromBody] CreateGroupDto dto)
    {
        var result = await service.Create(dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetGroupDto>> Put(string id, [FromBody] UpdateGroupDto dto)
    {
        return await service.Update(id, dto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await service.Delete(id);
        return NoContent();
    }
}