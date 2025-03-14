using System;
using System.Linq;
using System.Threading.Tasks;

using Acme.BookStore.Authors;


using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

using Xunit;

namespace Acme.BookStore.Books;

public abstract class BookAppService_Tests<TStartupModule> : BookStoreApplicationTestBase<TStartupModule> 
    where TStartupModule : IAbpModule
{
    private readonly IBookAppService _bookAppService;
    private readonly IAuthorAppService _authorAppService;


    public BookAppService_Tests()
    {
        _bookAppService = GetRequiredService<IBookAppService>();
        _authorAppService = GetRequiredService<IAuthorAppService>();
    }

    [Fact]
    public async Task Should_Get_List_Of_Books()
    {
        // Act
        var result = await _bookAppService.GetListAsync(new PagedAndSortedResultRequestDto());

        // Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(b => b.Name == "1984" &&
                                        b.AuthorName == "George Orwell");
    }

    [Fact]
    public async Task Should_Create_A_Valid_Book()
    {
        // Arange
        string bookName = "New test book 42";
        var authors = await _authorAppService.GetListAsync(new GetAuthorListDto());
        var firstAuthor = authors.Items[0];

        // Act
        var result = await _bookAppService.CreateAsync(new CreateUpdateBookDto
        {
            AuthorId = firstAuthor.Id,
            Name = bookName,
            PublishDate = DateTime.Now,
            Type = BookType.ScienceFiction,
            Price = 10f,
        });

        // Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(bookName);
    }

    [Fact]
    public async Task Should_Not_Create_A_Book_Without_Name()
    {
        // arrange
        string bookName = string.Empty;

        // act & assert
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _bookAppService.CreateAsync(new CreateUpdateBookDto
            {
                Name = bookName,
                PublishDate = DateTime.Now,
                Type = BookType.ScienceFiction,
                Price = 10f,
            });
        });

        exception.ValidationErrors.ShouldContain(err => err.MemberNames.Any(m => m == "Name"));
    }
}