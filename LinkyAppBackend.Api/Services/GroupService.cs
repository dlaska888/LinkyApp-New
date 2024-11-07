using AutoMapper;
using LinkyAppBackend.Api.Exceptions.Service;
using LinkyAppBackend.Api.Helpers.Interfaces;
using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Group;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Models.Entities.Assoc;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Enums;
using LinkyAppBackend.Api.Providers;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;

namespace LinkyAppBackend.Api.Services;

public class GroupService(
    AppDbContext dbContext,
    UserManager<AppUser> userManager,
    IPagingHelper pagingHelper,
    IMapper mapper,
    IAuthContextProvider auth
) : IService<GetGroupDto, CreateGroupDto, UpdateGroupDto>
{
    public async Task<GetGroupDto> GetById(string id)
    {
        var group = await FindGroupAndVerifyAccess(id);
        return mapper.Map<GetGroupDto>(group);
    }

    public async Task<PagedResults<GetGroupDto>> GetAll(SieveModel query)
    {
        var userId = auth.GetUserId();
        var groupsQuery = dbContext.LinkGroups
            .Where(g => g.Users.Any(u => u.UserId == userId))
            .AsNoTracking();

        return await pagingHelper.ToPagedResults<LinkGroup, GetGroupDto>(groupsQuery, query);
    }

    public async Task<GetGroupDto> Create(CreateGroupDto dto)
    {
        var user = await userManager.FindByIdAsync(auth.GetUserId());

        var newGroup = mapper.Map<LinkGroup>(dto);
        newGroup.Users.Add(new LinkGroupUser
        {
            UserId = user!.Id,
            Role = GroupRole.Owner
        });

        dbContext.LinkGroups.Add(newGroup);
        await dbContext.SaveChangesAsync();

        return mapper.Map<GetGroupDto>(newGroup);
    }

    public async Task<GetGroupDto> Update(string id, UpdateGroupDto dto)
    {
        var group = await FindGroupAndVerifyAccess(id, GroupRole.Owner);

        mapper.Map(dto, group);

        dbContext.LinkGroups.Update(group);
        await dbContext.SaveChangesAsync();

        return mapper.Map<GetGroupDto>(group);
    }

    public async Task<bool> Delete(string id)
    {
        var group = await FindGroupAndVerifyAccess(id, GroupRole.Owner);

        dbContext.LinkGroups.Remove(group);
        await dbContext.SaveChangesAsync();

        return true;
    }

    private async Task<LinkGroup> FindGroupAndVerifyAccess(string groupId, GroupRole role = GroupRole.Viewer)
    {
        var group = await dbContext.LinkGroups
            .Include(g => g.Users)
            .FirstOrDefaultAsync(g => g.Id == groupId);

        if (group == null)
            throw new NotFoundException("Group not found");

        VerifyGroupAccess(group, role);

        return group;
    }

    private void VerifyGroupAccess(LinkGroup group, GroupRole role = GroupRole.Viewer)
    {
        var userId = auth.GetUserId();
        if (!group.Users.Any(u => u.UserId == userId && u.Role >= role))
            throw new ForbiddenException("Your role in this group does not allow this action");
    }
}