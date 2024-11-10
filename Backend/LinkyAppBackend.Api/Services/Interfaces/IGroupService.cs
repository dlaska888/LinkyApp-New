using LinkyAppBackend.Api.Models.Dtos.Group;

namespace LinkyAppBackend.Api.Services.Interfaces;

public interface IGroupService : IService<GetGroupDto, CreateGroupDto, UpdateGroupDto>;
