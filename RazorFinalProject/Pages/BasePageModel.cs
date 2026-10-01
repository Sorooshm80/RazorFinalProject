using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class BasePageModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        protected int? CustomerId => HttpContext.Session.GetInt32("CustomerId");
        protected bool IsAdmin => HttpContext.Session.GetString("Role") == "Admin";
    }
}
