using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LegalLib.Data;
using LegalLib.Models;

namespace LegalLib
{
    public class DKListModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DKListModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IList<tblDocKlasifikasi> tblDocKlasifikasi { get;set; }
        [BindProperty]
        public tblDocKlasifikasi tblAddDK { get; set; }

        public int DocumentID { get; set; }
        [BindProperty]
        public SelectList KlasifikasiSL { get; set; }

        public void PopulateKlasifikasi()
        {
            var CatQuery = from d in _context.tblKlasifikasi
                           where d.IsActive == true
                           orderby d.KlasifikasiID
                           select d;

            KlasifikasiSL = new SelectList(CatQuery,"KlasifikasiID", "Klasifikasi");
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentID = id.Value;
            PopulateKlasifikasi();
            tblDocKlasifikasi = await _context.tblDocKlasifikasi.Where(m => m.DocumentID == id).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            tblAddDK.DocumentID = DocumentID;
            _context.tblDocKlasifikasi.Add(tblAddDK);
            await _context.SaveChangesAsync();

            PopulateKlasifikasi();

            return Page();
        }
    }
}
