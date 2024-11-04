using AutoMapper;
using LinkyAppBackend.Api.Models.Dtos.Account;
using LinkyAppBackend.Api.Models.Entities.Master;

namespace LinkyAppBackend.Api.AutoMapper;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<AppUser, GetAccountDto>();
    }
}