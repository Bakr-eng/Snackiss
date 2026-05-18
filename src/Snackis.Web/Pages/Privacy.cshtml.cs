using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace Snackis.Web.Pages
{
    [Authorize(Policy = "ShouldBeAdmin")]
    public class PrivacyModel : PageModel
    {
       
        public void OnGet()
        {
        }
    }

}
