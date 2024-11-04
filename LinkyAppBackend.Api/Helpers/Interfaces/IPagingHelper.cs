using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Interfaces;
using Sieve.Models;

namespace LinkyAppBackend.Api.Helpers.Interfaces;

public interface IPagingHelper
{
    public Task<PagedResults<TGetDto>> ToPagedResults<TEntity, TGetDto>(
        IQueryable<TEntity> records,
        SieveModel query,
        Func<TEntity, Task<TGetDto>>? customMapping = null)
        where TGetDto : IGetDto;
}