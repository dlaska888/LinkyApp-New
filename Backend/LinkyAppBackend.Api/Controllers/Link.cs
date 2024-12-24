using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Link;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace LinkyAppBackend.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/group/{groupId}/[controller]")]
public class Link(ILinkService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResults<GetLinkDto>>> Get(string groupId, [FromQuery] SieveModel query)
    {
        return await service.GetAll(groupId, query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetLinkDto>> Get(string groupId, string id)
    {
        return await service.GetById(groupId, id);
    }

    [HttpPost]
    public async Task<ActionResult<GetLinkDto>> Post(string groupId, [FromBody] CreateLinkDto dto)
    {
        var result = await service.Create(groupId, dto);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetLinkDto>> Put(string groupId, string id, [FromBody] UpdateLinkDto dto)
    {
        return await service.Update(groupId, id, dto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string groupId, string id)
    {
        await service.Delete(groupId, id);
        return NoContent();
    }
}