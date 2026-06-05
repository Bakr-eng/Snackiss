using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Application.Services;
using Snackis.Domain.Entities;
using System.Threading.Tasks;

namespace Snackis.Web.Pages.Admin
{
    [Authorize(Policy = "ShouldBeAdmin")]
    public class ReportedPageModel : PageModel
    {
        private readonly IReportService _reportService;

        public List<Report> Reports { get; set; } = new();

        public ReportedPageModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task OnGetAsync()
        {
            Reports = await _reportService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostMarkHandledAsync(int id)
        {
            await _reportService.MarkAsHandledAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _reportService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
