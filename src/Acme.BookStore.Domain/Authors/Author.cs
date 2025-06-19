using System;
using JetBrains.Annotations;

using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Authors;

public class Author : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set;}
    public DateTime BirthDate { get; set;}
    public string? ShortBio { get; set; }
    
    protected Author() { }

    public Author(
        Guid id,
        [NotNull] string name,
        DateTime birthDate,
        [CanBeNull] string? shortBio = null) : base(id)
    {
        Name = name;
        BirthDate = birthDate;
        ShortBio = shortBio;
    }

    public Author ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: AuthorConsts.MaxNameLength
        );
    }

}