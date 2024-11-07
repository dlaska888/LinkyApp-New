using AutoMapper;
using LinkyAppBackend.Api.Exceptions.Service;
using LinkyAppBackend.Api.Helpers.Interfaces;
using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.GroupUser;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Models.Entities.Assoc;
using LinkyAppBackend.Api.Models.Enums;
using LinkyAppBackend.Api.Providers;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using LinkyAppBackend.Api.Models.Entities.Master;

namespace LinkyAppBackend.Api.Services;

public class GroupUserService(
    AppDbContext dbContext,
    IAuthContextProvider authContext,
    UserManager<AppUser> userManager,
    IPagingHelper pagingHelper,
    IMapper mapper) : IGroupUserService
{
    public async Task<GetGroupUserDto> GetById(string groupId, string userId)
    {
        var userGroup = await dbContext.LinkGroupUsers
            .Include(x => x.User)
            .Include(x => x.Group)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);

        if (userGroup == null)
            throw new NotFoundException("User group not found");

        await VerifyGroupAccess(userGroup.GroupId);

        return mapper.Map<GetGroupUserDto>(userGroup);
    }

    public async Task<PagedResults<GetGroupUserDto>> GetAll(string groupId, SieveModel query)
    {
        await VerifyGroupAccess(groupId);

        var groupUsers = dbContext.LinkGroupUsers
            .Include(x => x.User)
            .Include(x => x.Group)
            .Where(x => x.GroupId == groupId);

        return await pagingHelper.ToPagedResults<LinkGroupUser, GetGroupUserDto>(groupUsers, query);
    }

    public async Task<GetGroupUserDto> Create(string groupId, CreateGroupUserDto dto)
    {
        var group = await FindGroup(groupId);
        await VerifyGroupAccess(groupId, dto.Role);

        var user = await userManager.FindByNameAsync(dto.UserName) ??
                   await userManager.FindByEmailAsync(dto.UserName);

        if (user is null)
            throw new NotFoundException("User not found");

        if (group.Users.Any(x => x.UserId == user.Id))
            throw new BadRequestException("User already in group");

        if (dto.Role == GroupRole.Owner)
            throw new BadRequestException("You cannot add owner to the group");

        var userGroup = new LinkGroupUser
        {
            UserId = user.Id,
            GroupId = group.Id,
            Role = dto.Role
        };

        dbContext.LinkGroupUsers.Add(userGroup);
        await dbContext.SaveChangesAsync();

        return mapper.Map<GetGroupUserDto>(userGroup);
    }

    public async Task<GetGroupUserDto> Update(string groupId, string userId, UpdateGroupUserDto dto)
    {
        var currUserId = authContext.GetUserId();
        var group = await FindGroup(groupId);

        var authGroupUser = group.Users.FirstOrDefault(x => x.UserId == currUserId)!;
        var otherGroupUser = group.Users.FirstOrDefault(x => x.UserId == userId);

        if (otherGroupUser is null)
            throw new NotFoundException("User not found in group");

        if (otherGroupUser.Role == dto.Role)
            throw new BadRequestException("User already has this role");

        if (otherGroupUser.Role == GroupRole.Owner || dto.Role == GroupRole.Owner)
            throw new BadRequestException("You cannot change owner role");

        if (authGroupUser.Role < otherGroupUser.Role || authGroupUser.Role < dto.Role)
            throw new ForbiddenException("You are not authorized to perform this action");

        mapper.Map(dto, otherGroupUser);
        await dbContext.SaveChangesAsync();

        return mapper.Map<GetGroupUserDto>(otherGroupUser);
    }

    public async Task<bool> Delete(string groupId, string userId)
    {
        var authUserId = authContext.GetUserId();
        var group = await FindGroup(groupId);

        var authGroupUser = group.Users.FirstOrDefault(x => x.UserId == authUserId)!;
        var otherGroupUser = group.Users.FirstOrDefault(x => x.UserId == userId);

        if (otherGroupUser is null)
            throw new NotFoundException("User not found in group");

        if (otherGroupUser.Role == GroupRole.Owner)
            throw new BadRequestException("You cannot remove owner from the group");

        if (authGroupUser.Role < otherGroupUser.Role)
            throw new ForbiddenException("You are not authorized to perform this action");

        dbContext.LinkGroupUsers.Remove(otherGroupUser);

        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task VerifyGroupAccess(string groupId, GroupRole role = GroupRole.Viewer)
    {
        var userId = authContext.GetUserId();

        var group = await FindGroup(groupId);

        if (group.Users.All(x => x.UserId != userId || x.Role < role))
            throw new ForbiddenException("You are not authorized to perform this action");
    }

    #region Private Methods

    private async Task<LinkGroup> FindGroup(string groupId)
    {
        var group = await dbContext.LinkGroups
            .Include(x => x.Users)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == groupId);

        if (group == null)
            throw new NotFoundException("Group not found");

        return group;
    }

    #endregion
}