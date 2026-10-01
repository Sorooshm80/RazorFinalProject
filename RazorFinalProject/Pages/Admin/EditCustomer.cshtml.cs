using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin
{
    public class EditCustomerModel : BasePageModel
    {
        private readonly ICustomerService _customers;

        public EditCustomerModel(ICustomerService customers)
        {
            _customers = customers;
        }

        [BindProperty] public int Id { get; set; }
        [BindProperty] public string FullName { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Phone { get; set; }
        [BindProperty] public bool IsVip { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            var customer = _customers.GetById(id);
            if (customer == null) return RedirectToPage("Customers");

            Id = customer.Id;
            FullName = customer.FullName;
            Email = customer.Email;
            Phone = customer.Phone;
            IsVip = customer.IsVip;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!IsAdmin) return RedirectToPage("/Login");
            var result = _customers.Update(Id, FullName, Email, Phone, IsVip);
            Message = result.Message;
            if (result.Success)
                return RedirectToPage("Customers");
            return Page();
        }
    }
}
