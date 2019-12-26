using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class DocDetailsModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocDetailsModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public tblLegalDocument tblLegalDocument { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblLegalDocument = await _context.tblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);

            if (tblLegalDocument == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
