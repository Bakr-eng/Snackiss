using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snackis.Web.Pages
{
    [Authorize(Policy = "ShouldBeAdmin")]
    public class AdminPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
