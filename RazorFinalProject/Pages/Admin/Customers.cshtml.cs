using Business;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin
{
    public class CustomersModel : BasePageModel
    {
        private readonly ICustomerService _customers;

        public CustomersModel(ICustomerService customers)
        {
            _customers = customers;
        }

        public List<Customer> Customers { get; set; }

        public IActionResult OnGet()
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            Customers = _customers.GetAll();
            return Page();
        }
    }
}
