using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class SignUpModel : BasePageModel
    {
        private readonly IAuthService _auth;

        public SignUpModel(IAuthService auth)
        {
            _auth = auth;
        }

        [BindProperty] public string FullName { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Phone { get; set; }
        [BindProperty] public string Password { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            var result = _auth.SignUp(FullName, Email, Phone, Password);
            Message = result.Message;
            if (result.Success)
                return RedirectToPage("/Login");
            return Page();
        }
    }
}
