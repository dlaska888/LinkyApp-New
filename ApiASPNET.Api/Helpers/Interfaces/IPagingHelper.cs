using ApiASPNET.Api.Models.Dtos;
using ApiASPNET.Api.Models.Dtos.Interfaces;
using Sieve.Models;

namespace ApiASPNET.Api.Helpers.Interfaces;

public interface IPagingHelper
{
    public Task<PagedResults<TGetDto>> ToPagedResults<TEntity, TGetDto>(
        IQueryable<TEntity> records,
        SieveModel query,
        Func<TEntity, Task<TGetDto>>? customMapping = null)
        where TGetDto : IGetDto;
}