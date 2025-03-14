using Acme.BookStore.Authors;
using Acme.BookStore.Books;

using AutoMapper;

namespace Acme.BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name));
        CreateMap<BookDto, Book>();
        CreateMap<BookDto, CreateUpdateBookDto>();
        CreateMap<CreateUpdateBookDto, Book>();

        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDto, Author>();
        CreateMap<AuthorDto, CreateAuthorDto>();
        CreateMap<AuthorDto, UpdateAuthorDto>();
        CreateMap<CreateAuthorDto, Author>();
        CreateMap<CreateAuthorDto, AuthorDto>();
        CreateMap<UpdateAuthorDto, Author>();
        CreateMap<UpdateAuthorDto, AuthorDto>();

        CreateMap<Author, AuthorLookupDto>();
    }
}
