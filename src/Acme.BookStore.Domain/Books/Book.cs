using System;

using Acme.BookStore.Authors;

using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Books;

public class Book(string name, BookType type, DateTime publishDate, float price, Guid authorId)
    : AuditedAggregateRoot<Guid>(Guid.NewGuid()), IMultiTenant
{
    public string Name { get; set; } = name;
    public BookType Type { get; set; } = type;
    public DateTime PublishDate { get; set; } = publishDate;
    public float Price { get; set; } = price;
    public Guid AuthorId { get; set; } = authorId;
    public virtual Author? Author { get; set; } 

    public Guid? TenantId { get; set; }
    public override DateTime CreationTime { get; set; }

}