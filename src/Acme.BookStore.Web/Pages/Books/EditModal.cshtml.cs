using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


using Acme.BookStore.Books;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.BookStore.Web.Pages.Books;

public class EditModalModel(IBookAppService bookAppService) : BookStorePageModel
{
    [BindProperty]
    public EditBookViewModel Book { get; set; } = new();
    public List<SelectListItem> Authors { get; set;} = [];

    private readonly IBookAppService _bookAppService = bookAppService;

    public async Task OnGetAsync(Guid id)
    {
        var bookDto = await _bookAppService.GetAsync(id);
        Book = ObjectMapper.Map<BookDto, EditBookViewModel>(bookDto);

        var authorLookup = await _bookAppService.GetAuthorLookupAsync();
        Authors = authorLookup.Items
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _bookAppService.UpdateAsync(
            Book.Id, 
            ObjectMapper.Map<EditBookViewModel, CreateUpdateBookDto>(Book));
        return NoContent();
    }

    public class EditBookViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [SelectItems(nameof(Authors))]
        [DisplayName("Author")]
        public Guid AuthorId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public float Price { get; set; }
    }
}