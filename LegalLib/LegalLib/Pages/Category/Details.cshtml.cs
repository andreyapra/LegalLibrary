using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CategoryDetailsModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CategoryDetailsModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public tblCategory tblCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblCategory = await _context.tblCategory.FirstOrDefaultAsync(m => m.CategoryID == id);

            if (tblCategory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
