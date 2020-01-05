using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace LegalLib
{
    public class AddKlasifikasiModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public AddKlasifikasiModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public int DocumentID { get; set; }
        [BindProperty]
        public IList<tblKlasifikasi> KlasifikasiSL { get; set; }
        [BindProperty]
        public List<tblDK> tblDocKlasifikasi {get;set;}

        public void PopulateKlasifikasi()
        {
            var DocQuery = from d in _context.tblKlasifikasi
                           orderby d.Klasifikasi
                           select d;

            KlasifikasiSL = new List<tblKlasifikasi>(DocQuery);

        }
        public IActionResult OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentID = id.Value;
            var DocQuery = from d in _context.tblDK
                           where d.DocumentID == DocumentID
                           select d;

            tblDocKlasifikasi = new List<tblDK>(DocQuery);

            return Page();
        }

    }
}
