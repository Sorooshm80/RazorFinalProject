using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class LoginModel : BasePageModel
    {
        private readonly IAuthService _auth;

        public LoginModel(IAuthService auth)
        {
            _auth = auth;
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string AdminPassword { get; set; }

        public void OnGet() { }

        public IActionResult OnPostCustomer()
        {
            var customer = _auth.Login(Email, Password);
            if (customer == null)
            {
                Message = "Wrong email or password.";
                return Page();
            }

            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            HttpContext.Session.SetString("CustomerName", customer.FullName);
            return RedirectToPage("/Member/Pools");
        }

        public IActionResult OnPostAdmin()
        {
            if (!_auth.AdminLogin(AdminPassword))
            {
                Message = "Wrong admin password.";
                return Page();
            }

            HttpContext.Session.SetString("Role", "Admin");
            return RedirectToPage("/Admin/Customers");
        }
    }
}
