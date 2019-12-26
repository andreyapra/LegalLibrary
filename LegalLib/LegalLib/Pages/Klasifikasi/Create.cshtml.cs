using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LegalLib
{
    public class KlasifikasiCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public KlasifikasiCreateModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public tblKlasifikasi tblKlasifikasi { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.tblKlasifikasi.Add(tblKlasifikasi);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
