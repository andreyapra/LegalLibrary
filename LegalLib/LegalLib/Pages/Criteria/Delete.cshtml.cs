using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CriteriaDeleteModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CriteriaDeleteModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public tblCriteria tblCriteria { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblCriteria = await _context.tblCriteria.FirstOrDefaultAsync(m => m.CriteriaID == id);

            if (tblCriteria == null)
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

            tblCriteria = await _context.tblCriteria.FindAsync(id);

            if (tblCriteria != null)
            {
                _context.tblCriteria.Remove(tblCriteria);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
