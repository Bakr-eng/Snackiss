using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application;
using Snackis.Application.Service;
using Snackis.Domain.Entities;

namespace Snackis.Web.Pages.Admin
{
    [Authorize(Policy = "ShouldBeAdmin")]
    public class CategoriesModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CategoriesModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public List<Category> Categories { get; set; } = new();

        [BindProperty] public string NewCategoryName { get; set; } = "";
        [BindProperty] public int? NewParentCategoryId { get; set; }
        [BindProperty] public int EditId { get; set; }
        [BindProperty] public string EditName { get; set; } = "";

        public async Task OnGetAsync()
        {
            Categories = await _categoryService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!string.IsNullOrWhiteSpace(NewCategoryName))
                await _categoryService.CreateAsync(NewCategoryName, NewParentCategoryId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!string.IsNullOrWhiteSpace(EditName))
                await _categoryService.UpdateAsync(EditId, EditName);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}