using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class KlasifikasiEditModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public KlasifikasiEditModel(LegalLib.Data.LegalLibContext context)
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(tblKlasifikasi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblKlasifikasiExists(tblKlasifikasi.KlasifikasiID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool tblKlasifikasiExists(int id)
        {
            return _context.tblKlasifikasi.Any(e => e.KlasifikasiID == id);
        }
    }
}
