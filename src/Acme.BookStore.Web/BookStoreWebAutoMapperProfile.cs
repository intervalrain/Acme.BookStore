using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using CreateBookViewModel = Acme.BookStore.Web.Pages.Books.CreateModalModel.CreateBookViewModel;
using CreateAuthorViewModel = Acme.BookStore.Web.Pages.Authors.CreateModalModel.CreateAuthorViewModel;
using EditAuthorViewModel = Acme.BookStore.Web.Pages.Authors.EditModalModel.EditAuthorViewModel;
using EditBookViewModel = Acme.BookStore.Web.Pages.Books.EditModalModel.EditBookViewModel;
using AutoMapper;

namespace Acme.BookStore.Web;

public class BookStoreWebAutoMapperProfile : Profile
{
    public BookStoreWebAutoMapperProfile()
    {
        CreateMap<BookDto, CreateUpdateBookDto>();
        CreateMap<CreateAuthorViewModel, CreateAuthorDto>();
        CreateMap<AuthorDto, EditAuthorViewModel>();
        CreateMap<EditAuthorViewModel, UpdateAuthorDto>();
        CreateMap<CreateBookViewModel, CreateUpdateBookDto>();
        CreateMap<EditBookViewModel, CreateUpdateBookDto>();
        CreateMap<BookDto, EditBookViewModel>();
    }
}
