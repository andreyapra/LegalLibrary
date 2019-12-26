using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CategoryIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CategoryIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IList<tblCategory> tblCategory { get; set; }

        public async Task OnGetAsync()
        {
            tblCategory = await _context.tblCategory.ToListAsync();
        }
    }
}
