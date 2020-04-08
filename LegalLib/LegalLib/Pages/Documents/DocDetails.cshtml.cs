using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class DocDetailsModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }
        public DocDetailsModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;

        }
        public List<TblCategory> TblCategory { get; set; }

        public TblLegalDocument TblLegalDocument { get; set; }
        public List<TblDK> TblDK { get; set; }
        public List<TblFileAttach> TblFileAttach { get; set; }
        public string SUsername { get; set; }

        public string GetKlasifikasi(int id)
        {
            string Klasifikasi;
            Klasifikasi = _context.TblKlasifikasi.Where(m => m.KlasifikasiID == id).FirstOrDefault().Klasifikasi;

            return Klasifikasi;
        }
        public string GetCriteria(int id)
        {
            string Criteria;
            Criteria = _context.TblCriteria.Where(m => m.CriteriaID == id).FirstOrDefault().Criteria;

            return Criteria;
        }
        public string GetCategory(int id)
        {
            string Category;
            Category = _context.TblCategory.Where(m => m.CategoryID == id).FirstOrDefault().Category;

            return Category;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            SUsername = HttpContext.Session.GetString("SUsername");

            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            TblLegalDocument = await _context.TblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);
            TblDK = await _context.TblDK.Where(m => m.DocumentID == id).Where(m => m.IsActive == true).ToListAsync();
            TblFileAttach = await _context.TblFileAttach.Where(m => m.DocumentID == id).Where(m => m.IsActive == true).ToListAsync();

            if (TblLegalDocument == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
