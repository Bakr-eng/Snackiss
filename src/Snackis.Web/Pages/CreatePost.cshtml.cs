using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.Services;
using Snackis.Domain.Entities;
using System.Security.Claims;

namespace Snackis.Web.Pages
{
    [Authorize(Policy = "ShouldBeUser")]
    public class CreatePostModel : PageModel
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public CreatePostModel(IPostService postService, ICategoryService categoryService, IWebHostEnvironment env)
        {
            _postService = postService;
            _categoryService = categoryService;
            _env = env;
        }

        public Category? SubCategory { get; set; }
        [BindProperty] public string Title { get; set; } = "";
        [BindProperty] public string Content { get; set; } = "";
        [BindProperty] public int CategoryId { get; set; }
        [BindProperty] public int ParentId { get; set; }
        [BindProperty] public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int categoryId, int parentId)
        {
            var allCategories = await _categoryService.GetAllAsync();
            var parent = allCategories.FirstOrDefault(c => c.Id == parentId);
            SubCategory = parent?.SubCategories.FirstOrDefault(s => s.Id == categoryId);

            if (SubCategory == null)
                return RedirectToPage("/Index");

            CategoryId = categoryId;
            ParentId = parentId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            string? imageUrl = null;

            if (Image != null && Image.Length > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(Image.FileName).ToLowerInvariant();

                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("Image", "Endast jpg, png, gif och webp är tillåtna.");
                    return Page();
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "posts");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await Image.CopyToAsync(stream);

                imageUrl = $"/uploads/posts/{fileName}";
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            await _postService.CreateAsync(Title, Content, CategoryId, userId, imageUrl);

            return RedirectToPage("/Index", new { parentId = ParentId, subId = CategoryId });
        }
    }
}