using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books;

public class BookAppService : 
    CrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateBookDto>,
    IBookAppService
{
    private readonly IAuthorRepository _authorRepository;

    public BookAppService(IRepository<Book, Guid> repository, IAuthorRepository authorRepository)
        : base(repository)
    {
        // GetPolicyName = BookStorePermissions.Books.Default;
        // GetListPolicyName = BookStorePermissions.Books.Default;
        // CreatePolicyName = BookStorePermissions.Books.Create;
        // DeletePolicyName = BookStorePermissions.Books.Default;
        // UpdatePolicyName = BookStorePermissions.Books.Edit;
        _authorRepository = authorRepository;
    }

    // public override async Task<BookDto> GetAsync(Guid id)
    // {
    //     var queryable = await Repository.GetQueryableAsync();

    //     var query = from book in queryable
    //                 join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
    //                 where book.Id == id
    //                 select new { book, author };

    //     var result = await AsyncExecuter.FirstOrDefaultAsync(query);
    //     if (result == null)
    //     {
    //         throw new EntityNotFoundException(typeof(Book), id);
    //     }

    //     var bookDto = ObjectMapper.Map<Book, BookDto>(result.book);
    //     bookDto.AuthorName = result.author.Name;
    //     return bookDto;
    // }

    // public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    // {
    //     var queryable = await Repository.GetQueryableAsync();

    //     var query = from book in queryable
    //                 join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
    //                 select new { book, author };

    //     query = query
    //         .OrderBy(NormalizeSorting(input.Sorting))
    //         .Skip(input.SkipCount)
    //         .Take(input.MaxResultCount);

    //     var result = await AsyncExecuter.ToListAsync(query);

    //     var bookDtos = result.Select(x => 
    //     {
    //         var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
    //         bookDto.AuthorName = x.author.Name;
    //         return bookDto;
    //     }).ToList();

    //     var totalCount = await Repository.GetCountAsync();

    //     return new PagedResultDto<BookDto>(
    //         totalCount,
    //         bookDtos
    //     );
    // }

    public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
    {
        var authors = await _authorRepository.GetListAsync();

        return new ListResultDto<AuthorLookupDto>(
            ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
        );
    }

    // private static string NormalizeSorting(string sorting)
    // {
    //     if (string.IsNullOrEmpty(sorting))
    //     {
    //         return $"book.{nameof(Book.Name)}";
    //     }

    //     if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
    //     {
    //         return sorting.Replace(
    //             "authorName",
    //             "author.Name",
    //             StringComparison.OrdinalIgnoreCase
    //         );
    //     }

    //     return $"book.{sorting}";
    // }
}