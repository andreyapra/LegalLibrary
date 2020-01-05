using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

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
        [BindProperty]
        public IList<tblDocKlasifikasi> tblDocKlasifikasi { get; set; }
        public List<tblDocKlasifikasi> tblDK { get; set; }

        public IList<tblFileAttach> tblFileAttach { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblLegalDocument = await _context.tblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);
            tblDocKlasifikasi = await _context.tblDocKlasifikasi.ToListAsync();
            tblFileAttach = await _context.tblFileAttach.ToListAsync();

            

            if (tblLegalDocument == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
