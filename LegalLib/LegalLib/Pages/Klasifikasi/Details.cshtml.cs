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

        public TblKlasifikasi TblKlasifikasi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblKlasifikasi = await _context.TblKlasifikasi.FirstOrDefaultAsync(m => m.KlasifikasiID == id);

            if (TblKlasifikasi == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
