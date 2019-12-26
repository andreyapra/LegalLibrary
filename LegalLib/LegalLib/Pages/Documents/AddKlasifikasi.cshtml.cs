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


        public List<tblKlasifikasi> KlasifikasiSL { get; set; }
        [BindProperty]
        public List<tblKlasifikasi> AreChecked { get; set; } = new List<tblKlasifikasi>();

        public void PopulateKlasifikasi()
        {
            var DocQuery = from d in _context.tblKlasifikasi
                           orderby d.Klasifikasi
                           select d;

            KlasifikasiSL = new List<tblKlasifikasi>(DocQuery);

        }
        public IActionResult OnGet()
        {
            PopulateKlasifikasi();

            return Page();
        }
        
        public IActionResult OnPost()
        {
            PopulateKlasifikasi();
            return Page();
        }
        
    }
}
