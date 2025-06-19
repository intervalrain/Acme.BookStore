using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Acme.BookStore.Permissions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Acme.BookStore.Authors;

[Authorize(BookStorePermissions.Authors.Default)]
public class AuthorAppService(IAuthorRepository repository, AuthorManager mgr) : BookStoreAppService, IAuthorAppService
{
    private readonly IAuthorRepository _repository = repository;
    private readonly AuthorManager _mgr = mgr;


    public async Task<AuthorDto> GetAsync(Guid id)
    {
        var author = await _repository.GetAsync(id);
        return ObjectMapper.Map<Author, AuthorDto>(author);
    }

    public async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
    {
        Logger.LogInformation("=============test=============");

        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Author.Name);
        }

        var authors = await _repository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );

        var totalCount = input.Filter == null
            ? await _repository.CountAsync()
            : await _repository.CountAsync(author => author.Name.Contains(input.Filter));

        return new PagedResultDto<AuthorDto>(
            totalCount,
            ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors)
        );
    }

    [Authorize(BookStorePermissions.Authors.Create)]
    public async Task<AuthorDto> CreateAsync(CreateAuthorDto input)
    {
        var author = await _mgr.CreateAsync(
            input.Name,
            input.BirthDate,
            input.ShortBio
        );

        await _repository.InsertAsync(author);
        return ObjectMapper.Map<Author, AuthorDto>(author);
    }

    [Authorize(BookStorePermissions.Authors.Edit)]
    public async Task UpdateAsync(Guid id, UpdateAuthorDto input)
    {
        var author = await _repository.GetAsync(id);

        if (author.Name != input.Name)
        {
            await _mgr.ChangeNameAsync(author, input.Name);
        }

        author.BirthDate = input.BirthDate;
        author.ShortBio = input.ShortBio;

        await _repository.UpdateAsync(author);
    }

    [Authorize(BookStorePermissions.Authors.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}