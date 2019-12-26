using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace LegalLib
{
    public class MenuModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public MenuModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IList<tblCategory> tblCategory { get; set; }

        public string SUsername { get; set; }

        public async Task OnGetAsync()
        {
            tblCategory = await _context.tblCategory.ToListAsync();

            SUsername = HttpContext.Session.GetString("SUsername");
            
        }
    }
}
