using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LegalLib
{
    public class LoginIndexModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            ViewData["Username"] = HttpContext.Session.GetString("SUsername");
        }

        public IActionResult OnPost()
        {
            HttpContext.Session.SetString("SUsername", Username);
            Message = HttpContext.Session.GetString("SUsername");

            switch (Username) 
            {
                case "public":
                    HttpContext.Session.SetInt32("SRole", 4);
                    HttpContext.Session.SetString("SNama", "Nama Public");
                    HttpContext.Session.SetString("SEmail", "public@email.com");
                    return Page();

                case "manager":
                    HttpContext.Session.SetInt32("SRole", 3);
                    HttpContext.Session.SetString("SNama", "Nama Manager");
                    HttpContext.Session.SetString("SEmail", "manager@email.com");
                    return Page();

                case "legal":
                    HttpContext.Session.SetInt32("SRole", 2);
                    HttpContext.Session.SetString("SNama", "Nama Legal");
                    HttpContext.Session.SetString("SEmail", "legal@email.com");
                    return Page();

                case "user":
                    HttpContext.Session.SetInt32("SRole", 1);
                    HttpContext.Session.SetString("SNama", "Nama User");
                    HttpContext.Session.SetString("SEmail", "user@email.com");
                    return Page();

                default:
                    Message = "Invalid username and password";
                    return Page();
            }
        }
    }
}