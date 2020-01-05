using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace LegalLib
{
    public class DocDetailsModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocDetailsModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public tblLegalDocument tblLegalDocument { get; set; }
        [BindProperty]
        public IList<tblDK> tblDocKlasifikasi { get; set; }
        public List<tblDK> tblDK { get; set; }
        public IList<tblFileAttach> tblFileAttach { get; set; }

        public int DocumentID { get; set; }
        public string Klasifikasi { get; set; }
        public void PopulateDK()
        {
            var DKQuery = from d in _context.tblDK
                          where d.DocumentID == DocumentID
                          select d;

            tblDocKlasifikasi = new List<tblDK>(DKQuery);
        }
        public void PopulateFileAttach()
        {
            var DKQuery = from d in _context.tblFileAttach
                          where d.DocumentID == DocumentID
                          select d;

            tblFileAttach = new List<tblFileAttach>(DKQuery);
        }

        public string GetKlasifikasi(int id)
        {
            Klasifikasi = _context.tblKlasifikasi.FirstOrDefault(m => m.KlasifikasiID == id).Klasifikasi;

            return Klasifikasi;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DocumentID = id.Value;
            tblLegalDocument = await _context.tblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);
            PopulateDK();
            PopulateFileAttach();
                       

            if (tblLegalDocument == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
