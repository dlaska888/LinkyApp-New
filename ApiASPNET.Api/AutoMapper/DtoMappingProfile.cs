using ApiASPNET.Api.Models.Dtos.Account;
using ApiASPNET.Api.Models.Entities.Master;
using AutoMapper;

namespace ApiASPNET.Api.AutoMapper;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<AppUser, GetAccountDto>();
    }
}