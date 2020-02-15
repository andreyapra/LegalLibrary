using LegalLib.Models;
using Microsoft.AspNetCore.Http;
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
            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                Response.Redirect("/Login/Index");
            }
            else if (SRole < 2)
            {
                Response.Redirect("/Denied");
            }

            return Page();
        }

        [BindProperty]
        public TblKlasifikasi TblKlasifikasi { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TblKlasifikasi.IsActive = true;
            TblKlasifikasi.CreatedBy = HttpContext.Session.GetString("SUsername");
            TblKlasifikasi.CreatedDate = System.DateTime.Now;
            TblKlasifikasi.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblKlasifikasi.ModifiedDate = System.DateTime.Now;

            _context.TblKlasifikasi.Add(TblKlasifikasi);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
