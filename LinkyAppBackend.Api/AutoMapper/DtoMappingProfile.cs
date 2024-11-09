using AutoMapper;
using LinkyAppBackend.Api.Controllers;
using LinkyAppBackend.Api.Models.Dtos.Account;
using LinkyAppBackend.Api.Models.Dtos.Group;
using LinkyAppBackend.Api.Models.Dtos.GroupUser;
using LinkyAppBackend.Api.Models.Dtos.Link;
using LinkyAppBackend.Api.Models.Entities.Master;
using Link = LinkyAppBackend.Api.Models.Entities.Master.Link;

namespace LinkyAppBackend.Api.AutoMapper;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<AppUser, GetAccountDto>();
        
        //group
        CreateMap<LinkGroup, GetGroupDto>();
        CreateMap<CreateGroupDto, LinkGroup>();
        CreateMap<UpdateGroupDto, LinkGroup>();
        
        //link
        CreateMap<Link, GetLinkDto>();
        CreateMap<CreateLinkDto, Link>();
        CreateMap<UpdateLinkDto, Link>();
        
        //group user
        CreateMap<GroupUser, GetGroupUserDto>();
        CreateMap<CreateGroupUserDto, GroupUser>();
        CreateMap<UpdateGroupUserDto, GroupUser>();
    }
}