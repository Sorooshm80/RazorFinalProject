using Business;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages.Member
{
    public class PoolsModel : BasePageModel
    {
        private readonly IPoolService _pools;
        private readonly IReservationService _reservations;
        private readonly IEntryService _entries;

        public PoolsModel(IPoolService pools, IReservationService reservations, IEntryService entries)
        {
            _pools = pools;
            _reservations = reservations;
            _entries = entries;
        }

        public SelectList PoolOptions { get; set; }
        public List<SessionRow> Rows { get; set; } = new List<SessionRow>();

        [BindProperty(SupportsGet = true)] public int PoolId { get; set; }
        [BindProperty(SupportsGet = true)] public DateTime Date { get; set; }

        public IActionResult OnGet()
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            LoadData();
            return Page();
        }

        public IActionResult OnPostReserve(int sessionId)
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            Message = _reservations.Reserve(CustomerId.Value, sessionId, Date).Message;
            return Back();
        }

        public IActionResult OnPostCancel(int sessionId)
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            Message = _reservations.Cancel(CustomerId.Value, sessionId, Date).Message;
            return Back();
        }

        public IActionResult OnPostEnter(int sessionId)
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            Message = _entries.Enter(CustomerId.Value, sessionId).Message;
            return Back();
        }

        private IActionResult Back()
        {
            return RedirectToPage(new { PoolId, Date = Date.ToString("yyyy-MM-dd") });
        }

        private void LoadData()
        {
            var allPools = _pools.GetPools();
            PoolOptions = new SelectList(allPools, "Id", "Name");
            if (Date == default) Date = DateTime.Today;
            if (PoolId == 0 && allPools.Count > 0) PoolId = allPools[0].Id;
            Rows = _pools.GetSessionRows(PoolId, Date, CustomerId.Value);
        }
    }
}
