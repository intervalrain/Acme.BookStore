using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

using Acme.BookStore.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Acme.BookStore.Authors;

public class EfCoreAuthorRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
    : EfCoreRepository<BookStoreDbContext, Author, Guid>(dbContextProvider),
      IAuthorRepository
{
    public async Task<Author?> FindByNameAsync(string name)
    {
        var authors = await GetDbSetAsync();
        return await authors.FirstOrDefaultAsync(author => author.Name == name);
    }

    public async Task<List<Author>> GetListAsync(int skipCount, int maxResultCount, string sorting, string? filter = null)
    {
        var authors = await GetDbSetAsync();
        return await authors
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                author => author.Name.Contains(filter)
            )
            .OrderBy(sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

}