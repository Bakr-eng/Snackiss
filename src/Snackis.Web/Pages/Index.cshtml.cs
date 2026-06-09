using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.Services;
using Snackis.Domain.Entities;
using System.Security.Claims;

namespace Snackis.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;
        private readonly IComentService _comentService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IReportService _reportService;

        public IndexModel(ICategoryService categoryService, IPostService postService,
            IComentService comentService, UserManager<AppUser> userManager, IReportService reportService)
        {
            _categoryService = categoryService;
            _postService = postService;
            _comentService = comentService;
            _userManager = userManager;
            _reportService = reportService;
        }

        public Dictionary<string, AppUser> UsersByPost { get; set; } = new(); // För att visa användarnamn på inlägg
        public Dictionary<string, AppUser> UsersByComent { get; set; } = new (); // För att visa användarnamn på kommentarer
        public Dictionary<int, List<Coment>> ComentsByPost { get; set; } = new(); // komentarer för varje post
        
        public List<Category> ParentCategories { get; set; } = new();
        public Category? SelectedParent { get; set; }
        public Category? SelectedSub { get; set; }
        public List<Post> Posts { get; set; } = new();


       


        [BindProperty(SupportsGet = true)] 
        public int? ParentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SubId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? OpenPostId { get; set; }

        [BindProperty]
        public string NewComentContent { get; set; } = string.Empty;

        [BindProperty]
        public int NewComentPostId { get; set; }

        
        [BindProperty]
        public int ReportPostId { get; set; }

        [BindProperty]
        public string ReportReason { get; set; } = ""; 

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

        public async Task<IActionResult> OnPostReportAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null && ReportPostId > 0 && !string.IsNullOrWhiteSpace(ReportReason))
            {
                await _reportService.CreateAsync(
                    ReportPostId,
                    userId,
                    ReportReason);
            }

            return RedirectToPage(new
            {
                parentId = ParentId,
                subId = SubId,
                openPostId = OpenPostId
            });
        }


        private async Task LoadPageDataAsync()
        {
            try
            {
                ParentCategories = await _categoryService.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid hämtning av kategorier: " + ex.Message);
                return;
            }


            if (ParentId.HasValue) 
            {
                SelectedParent = ParentCategories.FirstOrDefault(c => c.Id == ParentId);
            }


            if (SubId.HasValue) 
            {
                SelectedSub = SelectedParent?.SubCategories.FirstOrDefault(s => s.Id == SubId);

                if (SelectedSub != null)
                {
                    try
                    {
                        Posts = await _postService.GetByCategoryAsync(SubId.Value); 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Fel vid hämtning av inlägg: " + ex.Message);
                        return;
                    }

                    foreach (var post in Posts) // Ladda kommentarer för varje inlägg
                    {
                        try
                        {
                            ComentsByPost[post.Id] = await _comentService.GetByPostAsync(post.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Fel vid hämtning av kommentarer för inlägg {post.Id}: " + ex.Message);
                            ComentsByPost[post.Id] = new List<Coment>(); // Sätt en tom lista vid fel
                        }


                        try
                        {
                            if (!UsersByPost.ContainsKey(post.UserId)) // Ladda användarinformation för varje inlägg
                            {
                                var user = await _userManager.FindByIdAsync(post.UserId);
                                if (user != null)
                                {
                                    UsersByPost[post.UserId] = user;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Fel vid hämtning av användare för inlägg {post.Id}: " + ex.Message);
                        }

                        try
                        {
                            foreach (var coment in ComentsByPost[post.Id]) // Ladda användarinformation för varje kommentar
                            {
                                if (!UsersByComent.ContainsKey(coment.UserId))
                                {
                                    var user = await _userManager.FindByIdAsync(coment.UserId);
                                    if (user != null)
                                    {
                                        UsersByComent[coment.UserId] = user;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Fel vid hämtning av användare för kommentarer i inlägg {post.Id}: " + ex.Message);

                        }

                    }
                  
                }

            }
        }

    }
}
