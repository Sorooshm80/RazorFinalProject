using Business;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin
{
    public class ReportsModel : BasePageModel
    {
        private readonly IReportService _reports;

        public ReportsModel(IReportService reports)
        {
            _reports = reports;
        }

        [BindProperty] public int CustomerId1 { get; set; }
        [BindProperty] public int Days1 { get; set; } = 7;
        [BindProperty] public int Days2 { get; set; } = 7;
        [BindProperty] public int TopCount { get; set; } = 3;
        [BindProperty] public DateTime Date3 { get; set; } = DateTime.Today;
        [BindProperty] public int Days4 { get; set; } = 7;

        public List<VisitReportItem> Report1 { get; set; }
        public List<PoolCountItem> Report2 { get; set; }
        public PoolCountItem Report3 { get; set; }
        public bool Report3Done { get; set; }
        public List<MissedBookingItem> Report4 { get; set; }

        public IActionResult OnGet()
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            return Page();
        }

        public IActionResult OnPostVisits()
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            Report1 = _reports.CustomerVisits(CustomerId1, Days1);
            return Page();
        }

        public IActionResult OnPostTopPools()
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            Report2 = _reports.TopPools(Days2, TopCount);
            return Page();
        }

        public IActionResult OnPostBusiest()
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            Report3 = _reports.BusiestPoolOnDate(Date3);
            Report3Done = true;
            return Page();
        }

        public IActionResult OnPostMissed()
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            Report4 = _reports.MissedBookings(Days4);
            return Page();
        }
    }
}
