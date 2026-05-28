using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.Services;
using Snackis.Domain.Entities;
using System;
using System.Security.Claims;

namespace Snackis.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IComentService _comentService;

        public IndexModel(ICategoryService categoryService, IPostService postService, IComentService comentService)
        {
            _categoryService = categoryService;
            _postService = postService;
            _comentService = comentService;
        }

        public List<Category> ParentCategories { get; set; } = new();
        public Category? SelectedParent { get; set; }
        public Category? SelectedSub { get; set; }
        public List<Post> Posts { get; set; } = new();

        // komentarer för varje post
        public Dictionary<int, List<Coment>> ComentsByPost { get; set; } = new();


        [BindProperty(SupportsGet = true)]
        public int? ParentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SubId { get; set; }

        // Vilket inläggs kommentarsektion är öppen
        [BindProperty(SupportsGet = true)]
        public int? OpenPostId { get; set; }

        [BindProperty]
        public string NewComentContent { get; set; } = string.Empty;

        [BindProperty]
        public int NewComentPostId { get; set; }

        public async Task OnGetAsync(int? parentId, int? subId)
        {
           
            await LoadPageDataAsync();
        }

        public async Task<IActionResult> OnPostAddComentAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(NewComentContent) && userId != null)
            {
                await _comentService.CreateAsync(NewComentContent, NewComentPostId, userId);
            }
            // Gå tillbaka med samma vy-parametrar + öppet kommentarsfält
            return RedirectToPage(new
            {
                parentId = ParentId,
                subId = SubId,
                openPostId = NewComentPostId
            });
        }
        private async Task LoadPageDataAsync()
        {
            ParentCategories = await _categoryService.GetAllAsync();

            if (ParentId.HasValue)
            {
                SelectedParent = ParentCategories.FirstOrDefault(c => c.Id == ParentId);
            }


            if (SubId.HasValue)
            {
                SelectedSub = SelectedParent?.SubCategories.FirstOrDefault(s => s.Id == SubId);

                if (SelectedSub != null)
                {
                    Posts = await _postService.GetByCategoryAsync(SubId.Value);

                    // Ladda kommentarer för alla inlägg i kategorin
                    foreach (var post in Posts)
                    {
                        ComentsByPost[post.Id] = await _comentService.GetByPostAsync(post.Id);
                    }
                }

            }
        }
    }
}
