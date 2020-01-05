using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LegalLib.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string SUsername { get; set; }
        public void OnGet()
        {
            SUsername = HttpContext.Session.GetString("SUsername");

            Page();
        }
    }
}
