using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Acme.BookStore.Authors;

using Acme.BookStore.Books;

using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Acme.BookStore;

public class BookStoreDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;
    private readonly List<Author> _authors;
    private readonly List<Book> _books;

    public BookStoreDataSeederContributor(
        IRepository<Book, Guid> bookRepository, 
        IAuthorRepository authorRepository, 
        AuthorManager authorManager)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _authorManager = authorManager;

        _authors = 
        [
            new(Guid.NewGuid(), "George Orwell", new DateTime(1903, 06, 25),
                "Orwell produced literacy criticism and poetry, fiction and polemical journalism; and is best known for the allergical novella Animal Farm (1945) and dystopian novel Nineteen Eighty-Four (1949)."
            ),
            new(Guid.NewGuid(), "Douglas Adams", new DateTime(1952, 03, 11),
                "Douglas Adams was an English author, screenwriter, esaayist, humorist, satirist and dramatist. Adams was an advocate for environmentalism and conservation, a lover of fast cars, technological innovation and the Apple Macintosh, and a self-proclaimed 'radical atheist'."
            )
        ];

        _books =
        [
            new("1984", BookType.Dystopia, new DateTime(1949, 6, 8), 19.84f, _authors[0].Id),
            new("The Hitchhiker's Guide to Galaxy", BookType.ScienceFiction, new DateTime(1995, 9, 27), 42.0f, _authors[1].Id)
        ];
    }


    public IReadOnlyList<Book> Books => _books.AsReadOnly();
    public IReadOnlyList<Author> Authors => _authors.AsReadOnly();

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _bookRepository.GetCountAsync() > 0)
        {
            return;
        }
        await _authorRepository.InsertManyAsync(_authors, autoSave: true);
        await _bookRepository.InsertManyAsync(_books, autoSave: true);
    }
}