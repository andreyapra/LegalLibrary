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
        public IList<tblDK> tblDocKlasifikasi { get;set; }
        [BindProperty]
        public tblDK tblAddDK { get; set; }
        public string Klasifikasi { get; set; }
        [BindProperty]
        public int DocumentID { get; set; }
        [BindProperty]
        public int KlasifikasiID { get; set; }
        public SelectList KlasifikasiSL { get; set; }

        public void PopulateKlasifikasi()
        {
            var CatQuery = from d in _context.tblKlasifikasi
                           where d.IsActive == true
                           orderby d.KlasifikasiID
                           select d;

            KlasifikasiSL = new SelectList(CatQuery, "KlasifikasiID", "Klasifikasi");
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentID = id.Value;
            PopulateKlasifikasi();
            tblDocKlasifikasi = await _context.tblDK.Where(m => m.DocumentID == id).ToListAsync();

            return Page();
        }

        public string GetKlasifikasi(int id)
        {
            Klasifikasi = _context.tblKlasifikasi.FirstOrDefault(m => m.KlasifikasiID == id).Klasifikasi;

            return Klasifikasi;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            tblAddDK.KlasifikasiID = KlasifikasiID;
            tblAddDK.DocumentID = DocumentID;
            _context.tblDK.Add(tblAddDK);
            await _context.SaveChangesAsync();


            return Redirect("DKList?id="+DocumentID);
        }
    }
}
