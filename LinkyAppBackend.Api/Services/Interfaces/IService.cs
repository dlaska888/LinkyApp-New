using Sieve.Models;
using LinkyAppBackend.Api.Models;
using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Interfaces;

namespace LinkyAppBackend.Api.Services.Interfaces;

public interface IService<TGetDto, TCreateDto, TUpdateDto>
where TGetDto : IGetDto
{
    Task<TGetDto> GetById(string id);
    Task<PagedResults<TGetDto>> GetAll(SieveModel query);
    Task<TGetDto> Create(TCreateDto dto);
    Task<TGetDto> Update(string id, TUpdateDto dto);
    Task<bool> Delete(string id);
}