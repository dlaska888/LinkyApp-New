using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Link;
using LinkyAppBackend.Api.Services.Interfaces;
using LinkyAppBackend.Api.Helpers.Interfaces;
using AutoMapper;
using LinkyAppBackend.Api.Exceptions.Service;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Enums;
using LinkyAppBackend.Api.Providers;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;

namespace LinkyAppBackend.Api.Services;

public class LinkService(
    AppDbContext dbContext,
    IPagingHelper pagingHelper,
    IMapper mapper,
    IAuthContextProvider auth)
    : ILinkService
{
    public async Task<GetLinkDto> GetById(string groupId, string id)
    {
        var link = await FindLinkWithAccess(groupId, id);
        return mapper.Map<GetLinkDto>(link);
    }

    public async Task<PagedResults<GetLinkDto>> GetAll(string groupId, SieveModel query)
    {
        await FindGroupWithAccess(groupId);
        var linksQuery = dbContext.Links
            .Where(l => l.GroupId == groupId)
            .AsQueryable();

        return await pagingHelper.ToPagedResults<Link, GetLinkDto>(linksQuery, query);
    }

    public async Task<GetLinkDto> Create(string groupId, CreateLinkDto dto)
    {
        await FindGroupWithAccess(groupId, GroupRole.Editor);
        var newLink = mapper.Map<Link>(dto);
        newLink.GroupId = groupId;

        dbContext.Links.Add(newLink);
        await dbContext.SaveChangesAsync();

        return mapper.Map<GetLinkDto>(newLink);
    }

    public async Task<GetLinkDto> Update(string groupId, string id, UpdateLinkDto dto)
    {
        var link = await FindLinkWithAccess(groupId, id, GroupRole.ContentManager);

        mapper.Map(dto, link);

        dbContext.Links.Update(link);
        await dbContext.SaveChangesAsync();

        return mapper.Map<GetLinkDto>(link);
    }

    public async Task<bool> Delete(string groupId, string id)
    {
        var link = await FindLinkWithAccess(groupId, id, GroupRole.ContentManager);

        dbContext.Links.Remove(link);
        await dbContext.SaveChangesAsync();

        return true;
    }

    private async Task<Link> FindLinkWithAccess(string groupId, string id, GroupRole role = GroupRole.Viewer)
    {
        var link = await dbContext.Links
            .Include(l => l.Group)
            .ThenInclude(g => g.Users)
            .FirstOrDefaultAsync(l => l.GroupId == groupId && l.Id == id);

        if (link == null)
            throw new NotFoundException("Link not found.");
        
        VerifyLinkAccess(link, role);

        return link;
    }

    private async Task<LinkGroup> FindGroupWithAccess(string groupId, GroupRole role = GroupRole.Viewer)
    {
        var group = await dbContext.LinkGroups
            .FirstOrDefaultAsync(x => x.Id == groupId);

        if (group is null)
            throw new NotFoundException("Group not found");
        
        VerifyGroupAccess(group, role);

        return group;
    }

    private void VerifyLinkAccess(Link link, GroupRole role = GroupRole.Viewer)
    {
        var userId = auth.GetUserId();
        if (link.CreatorId != userId && !link.Group.Users.Any(u => u.UserId == userId && u.Role >= role))
            throw new ForbiddenException("Your role in this group does not allow this action.");
    }
    
    private void VerifyGroupAccess(LinkGroup group, GroupRole role = GroupRole.Viewer)
    {
        var userId = auth.GetUserId();
        if (!group.Users.Any(u => u.UserId == userId && u.Role >= role))
            throw new ForbiddenException("Your role in this group does not allow this action.");
    }
}