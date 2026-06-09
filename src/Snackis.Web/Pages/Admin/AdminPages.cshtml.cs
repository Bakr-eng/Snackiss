using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Snackis.Web.Pages.Admin
{
    [Authorize(policy: "ShouldBeAdmin")]
    public class AdminPagesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
