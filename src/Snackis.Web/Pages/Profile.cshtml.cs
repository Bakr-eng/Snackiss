using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Domain.Entities;

namespace Snackis.Web.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProfileModel(UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        [BindProperty] public IFormFile? ProfilePicture { get; set; }
        public AppUser? CurrentUser { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
        }


        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ProfilePicture != null)
            {                 
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var ext = Path.GetExtension(ProfilePicture.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("ProfilePicture", "Endast jpg, png, gif och webp är tillåtna.");
                    return Page();
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "ProfilePictures");
                Directory.CreateDirectory(uploadsFolder); // Skapar mappen om den inte finns
                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create)) 
                {
                    await ProfilePicture.CopyToAsync(stream); 
                }
                CurrentUser!.ProfilePictureUrl = $"/uploads/ProfilePictures/{fileName}"; 
                await _userManager.UpdateAsync(CurrentUser);
            }

            return RedirectToPage();
        }
    }
}
