using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.GroupUser;
using LinkyAppBackend.Api.Models.Enums;
using Sieve.Models;

namespace LinkyAppBackend.Api.Services.Interfaces;

public interface IGroupUserService
{
    Task<GetGroupUserDto> GetById(string groupId, string id);
    Task<PagedResults<GetGroupUserDto>> GetAll(string groupId, SieveModel query);
    Task<GetGroupUserDto> Create(string groupId, CreateGroupUserDto dto);
    Task<GetGroupUserDto> Update(string groupId, string id, UpdateGroupUserDto dto);
    Task<bool> Delete(string groupId, string id);

    Task VerifyGroupAccess(string groupId, GroupRole role = GroupRole.Viewer);
}