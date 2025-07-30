using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Live.MagicAuth.Pages
{
    public class AuthModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Operation{ get; set; }

        public IActionResult OnGet()
        {
            if (Operation != "login" && Operation != "register")
            {
                return NotFound(); // optional: redirect or return error
            }

            return Page();
        }
    }
}
