using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Link;
using Sieve.Models;

namespace LinkyAppBackend.Api.Services.Interfaces;

public interface ILinkService
{
    Task<GetLinkDto> GetById(string groupId, string id);
    Task<PagedResults<GetLinkDto>> GetAll(string groupId, SieveModel query);
    Task<GetLinkDto> Create(string groupId, CreateLinkDto dto);
    Task<GetLinkDto> Update(string groupId, string id, UpdateLinkDto dto);
    Task<bool> Delete(string groupId, string id);
}