using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.Service;
using Snackis.Domain.Entities;
using System.Security.Claims;

namespace Snackis.Web.Pages
{
    [Authorize(Policy = "ShouldBeUser")]
    public class CreatePostModel : PageModel
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public CreatePostModel(IPostService postService, ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        public Category? SubCategory { get; set; }

        [BindProperty] public string Title { get; set; } = "";
        [BindProperty] public string Content { get; set; } = "";
        [BindProperty] public int CategoryId { get; set; }
        [BindProperty] public int ParentId { get; set; }

        public async Task<IActionResult> OnGetAsync(int categoryId, int parentId)
        {
            var allCategories = await _categoryService.GetAllAsync();
            var parent = allCategories.FirstOrDefault(c => c.Id == parentId);
            SubCategory = parent?.SubCategories.FirstOrDefault(s => s.Id == categoryId);

            if (SubCategory == null)
            {
                return RedirectToPage("/Index");
            }

            CategoryId = categoryId;
            ParentId = parentId;
            return Page();

        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _postService.CreateAsync(Title, Content, CategoryId, userId);

            return RedirectToPage("/Index", new { parentId = ParentId, subId = CategoryId });
        }
    }
}
