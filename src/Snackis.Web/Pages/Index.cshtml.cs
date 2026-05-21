using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.Service;
using Snackis.Domain.Entities;
using Snackis.Web.Pages.Admin;

namespace Snackis.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public IndexModel(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        public List<Category> ParentCategories { get; set; } = new();
        public Category? SelectedParent {  get; set; }
        public Category? SelectedSub { get; set; }
        public List<Post> Posts { get; set; } = new();
        public async Task OnGetAsync(int? parentId, int? subId)
        {
            ParentCategories = await _categoryService.GetAllAsync();

            if(parentId.HasValue)
            {
                SelectedParent = ParentCategories.FirstOrDefault(c => c.Id == parentId);
            }

            if (subId.HasValue)
            {
                SelectedSub = SelectedParent?.SubCategories.FirstOrDefault(s => s.Id == subId);
                if (SelectedSub != null)
                {
                    Posts = await _postService.GetByCategoryAsync(subId.Value);
                }
            }
        }
    }
}
