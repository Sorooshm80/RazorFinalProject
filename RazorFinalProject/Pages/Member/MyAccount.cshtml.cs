using Business;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Member
{
    public class MyAccountModel : BasePageModel
    {
        private readonly IPackageService _packages;
        private readonly IReservationService _reservations;
        private readonly ICustomerService _customers;

        public MyAccountModel(IPackageService packages, IReservationService reservations,
                              ICustomerService customers)
        {
            _packages = packages;
            _reservations = reservations;
            _customers = customers;
        }

        public bool IsVip { get; set; }
        public List<PackagePlan> Plans { get; set; }
        public List<SessionPackage> MyPackages { get; set; }
        public List<Booking> MyBookings { get; set; }
        public int ActiveFixed { get; set; }
        public int ActiveFreeTime { get; set; }

        public IActionResult OnGet()
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            LoadData();
            return Page();
        }

        public IActionResult OnPostBuy(int planId)
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            Message = _packages.Buy(CustomerId.Value, planId).Message;
            return RedirectToPage();
        }

        public IActionResult OnPostCancel(int sessionId, DateTime date)
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            Message = _reservations.Cancel(CustomerId.Value, sessionId, date).Message;
            return RedirectToPage();
        }

        private void LoadData()
        {
            IsVip = _customers.GetById(CustomerId.Value).IsVip;
            Plans = _packages.GetPlans();
            MyPackages = _packages.GetPackages(CustomerId.Value);
            MyBookings = _reservations.GetBookings(CustomerId.Value);

            ActiveFixed = MyPackages.Where(p => !p.IsExpired).Sum(p => p.FixedRemaining);
            ActiveFreeTime = MyPackages.Where(p => !p.IsExpired).Sum(p => p.FreeTimeRemaining);
        }
    }
}
