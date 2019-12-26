using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace LegalLib
{
    public class KlasifikasiIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public KlasifikasiIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IList<tblKlasifikasi> tblKlasifikasi { get; set; }

        public async Task OnGetAsync()
        {
            tblKlasifikasi = await _context.tblKlasifikasi.ToListAsync();
        }
    }
}
