using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class KlasifikasiDetailsModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public KlasifikasiDetailsModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

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
    }
}
