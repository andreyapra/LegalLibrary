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

        public TblLegalDocument TblLegalDocument { get; set; }
        public List<TblDK> TblDK { get; set; }
        public List<TblFileAttach> TblFileAttach { get; set; }

        public string GetKlasifikasi(int id)
        {
            string Klasifikasi;
            Klasifikasi = _context.TblKlasifikasi.Where(m => m.KlasifikasiID == id).FirstOrDefault().Klasifikasi;

            return Klasifikasi;
        }

            public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblLegalDocument = await _context.TblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);
            TblDK = await _context.TblDK.Where(m => m.DocumentID == id).ToListAsync();
            TblFileAttach = await _context.TblFileAttach.Where(m => m.DocumentID == id).ToListAsync();

            if (TblLegalDocument == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
