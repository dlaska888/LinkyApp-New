﻿using AutoMapper;
using LinkyAppBackend.Api.Helpers.Interfaces;
using LinkyAppBackend.Api.Models.Dtos;
using LinkyAppBackend.Api.Models.Dtos.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace LinkyAppBackend.Api.Helpers;

public class PagingHelper(ISieveProcessor sieveProcessor, IMapper mapper)
    : IPagingHelper
{
    public async Task<PagedResults<TGetDto>> ToPagedResults<TEntity, TGetDto>(
        IQueryable<TEntity> records,
        SieveModel query,
        Func<TEntity, Task<TGetDto>>? customMapping = null)
        where TGetDto : IGetDto
    {
        var filtered = sieveProcessor.Apply(query, records, applyPagination: false, applySorting: false);
        var paged = sieveProcessor.Apply(query, filtered, applyFiltering: false);

        var mapped = await Task.WhenAll(paged.AsEnumerable().Select(async x =>
            customMapping != null
                ? await customMapping(x)
                : mapper.Map<TGetDto>(x)
        ));

        return new PagedResults<TGetDto>(
            mapped,
            query.Page.GetValueOrDefault(),
            query.PageSize.GetValueOrDefault(),
            await filtered.CountAsync()
        );
    }
}