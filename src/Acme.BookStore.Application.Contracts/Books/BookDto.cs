using System;

using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books;

public class BookDto : AuditedEntityDto<Guid>
{
    public Guid AuthorId { get; set; }
    public required string AuthorName { get; set; }
    public required string Name { get; set; }
    public BookType Type { get; set; }
    public DateTime PublishDate { get; set; }
    public float price { get; set; }
}