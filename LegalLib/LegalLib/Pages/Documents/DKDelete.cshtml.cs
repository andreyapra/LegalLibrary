using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LegalLib.Data;
using LegalLib.Models;

namespace LegalLib
{
    public class DKDeleteModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DKDeleteModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public tblDocKlasifikasi tblDocKlasifikasi { get; set; }

        public int DokumentID { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblDocKlasifikasi = await _context.tblDocKlasifikasi.FirstOrDefaultAsync(m => m.DKID == id);
            DokumentID = tblDocKlasifikasi.DocumentID;

            if (tblDocKlasifikasi == null)
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

            tblDocKlasifikasi = await _context.tblDocKlasifikasi.FindAsync(id);

            if (tblDocKlasifikasi != null)
            {
                _context.tblDocKlasifikasi.Remove(tblDocKlasifikasi);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./DKList?id=" + DokumentID);

        }
    }
}
