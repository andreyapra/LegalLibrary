using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CriteriaDetailsModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CriteriaDetailsModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public TblCriteria TblCriteria { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblCriteria = await _context.TblCriteria.FirstOrDefaultAsync(m => m.CriteriaID == id);

            if (TblCriteria == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
