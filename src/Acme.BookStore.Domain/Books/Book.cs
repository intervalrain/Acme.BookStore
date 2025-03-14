using System;

using Acme.BookStore.Authors;

using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Books;

public class Book(string name, BookType type, DateTime publishDate, float price, Guid authorId)
    : AuditedAggregateRoot<Guid>(Guid.NewGuid())
{
    public string Name { get; set; } = name;
    public BookType Type { get; set; } = type;
    public DateTime PublishDate { get; set; } = publishDate;
    public float Price { get; set; } = price;
    public Guid AuthorId { get; set; } = authorId;
    public virtual Author Author { get; set; }
}