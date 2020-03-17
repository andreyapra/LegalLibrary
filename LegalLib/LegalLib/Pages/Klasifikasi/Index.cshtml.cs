using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace LegalLib
{
    public class KlasifikasiIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public KlasifikasiIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public IList<TblKlasifikasi> TblKlasifikasi { get; set; }
        public List<TblCategory> TblCategory { get; set; }

        public string SUsername { get; set; }
        public int SRole { get; set; }

        public async Task OnGetAsync()
        {
            var Klasifikasi = from m in _context.TblKlasifikasi
                           select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                Klasifikasi = Klasifikasi.Where(s => s.Klasifikasi.Contains(SearchString));
            }

            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            TblKlasifikasi = await Klasifikasi.Where(t => t.IsActive == true).ToListAsync();

            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                Response.Redirect("Login");
            }
            else if (SRole < 2)
            {
                Response.Redirect("Denied");
            }

        }
    }
}
