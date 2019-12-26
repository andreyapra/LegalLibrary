using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CategoryDeleteModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CategoryDeleteModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblCategory = await _context.tblCategory.FindAsync(id);

            if (tblCategory != null)
            {
                _context.tblCategory.Remove(tblCategory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
