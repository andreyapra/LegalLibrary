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

        public TblCategory TblCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblCategory = await _context.TblCategory.FirstOrDefaultAsync(m => m.CategoryID == id);

            if (TblCategory == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
