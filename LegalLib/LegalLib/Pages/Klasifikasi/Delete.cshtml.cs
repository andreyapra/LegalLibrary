using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class KlasifikasiDeleteModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public KlasifikasiDeleteModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public tblKlasifikasi tblKlasifikasi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblKlasifikasi = await _context.tblKlasifikasi.FirstOrDefaultAsync(m => m.KlasifikasiID == id);

            if (tblKlasifikasi == null)
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

            tblKlasifikasi = await _context.tblKlasifikasi.FindAsync(id);

            if (tblKlasifikasi != null)
            {
                _context.tblKlasifikasi.Remove(tblKlasifikasi);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
