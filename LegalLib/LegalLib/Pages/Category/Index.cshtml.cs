using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LegalLib
{
    public class CategoryIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CategoryIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet =true)]
        public string SearchString { get; set; }
        public IList<TblCategory> TblCategory { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }
        

        public async Task OnGetAsync()
        {
            var Category = from m in _context.TblCategory
                           select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                Category = Category.Where(s => s.Category.Contains(SearchString));
            }
            
            TblCategory = await Category.Where(t => t.IsActive == true).ToListAsync();

            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                Response.Redirect("Login");
            }
            else if (SRole < 2)
            {
                Response.Redirect("Denied");
            }

        }
    }
}
