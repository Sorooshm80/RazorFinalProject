using Business;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public List<Pool> AllPools { get; set; }
        public List<PoolSession> Sessions { get; set; }

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
            var result = _reservations.Reserve(CustomerId.Value, sessionId, Date);
            Message = result.Message;
            return RedirectToPage(new { PoolId, Date = Date.ToString("yyyy-MM-dd") });
        }

        public IActionResult OnPostEnter(int sessionId)
        {
            if (CustomerId == null) return RedirectToPage("/Login");
            var result = _entries.Enter(CustomerId.Value, sessionId);
            Message = result.Message;
            return RedirectToPage(new { PoolId, Date = Date.ToString("yyyy-MM-dd") });
        }

        private void LoadData()
        {
            AllPools = _pools.GetPools();
            if (Date == default) Date = DateTime.Today;
            if (PoolId == 0 && AllPools.Count > 0) PoolId = AllPools[0].Id;
            Sessions = _pools.GetSessions(PoolId, Date);
        }
    }
}
