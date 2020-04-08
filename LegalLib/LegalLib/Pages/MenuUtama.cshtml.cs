using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class MenuModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;
        public IConfiguration Configuration { get; }

        public MenuModel(LegalLib.Data.LegalLibContext context, ILogger<MenuModel> logger, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        [BindProperty]
        public List<TblCategory> TblCategory { get; set; }
        [BindProperty]
        public List<TblLegalDocument> TblLegalDocument { get; set; }
        public string SUsername { get; set; }

        public List<TblDK> TblDocK { get; set; }

        public void PopulateDocument()
        {
            var DocQuery = (from d in _context.TblLegalDocument
                            where d.ApproveStatus == "APPROVE"
                            where d.Status != "CABUT"
                            where d.TglAkhir > System.DateTime.Today
                            where d.IsActive == true
                           orderby d.TglMulai descending
                           select d).Take(10);

            TblLegalDocument = DocQuery.ToList();
        }

        public void PopulateDK()
        {
            var KQuery = from d in _context.TblDK
                         select d;

            TblDocK = new List<TblDK>(KQuery);

        }

        public string GetKlasifikasi(int id)
        {
            string Klasifikasi;
            Klasifikasi = _context.TblKlasifikasi.Where(m => m.KlasifikasiID == id).FirstOrDefault().Klasifikasi;

            return Klasifikasi;
        }
        public string GetCategory(int id)
        {
            string Category;
            Category = _context.TblCategory.Where(m => m.CategoryID == id).FirstOrDefault().Category;

            return Category;
        }

        public async Task OnGetAsync()
        {
            ViewData["URL"] = Configuration["Setting:ExternalURL"];
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            PopulateDocument();
            PopulateDK();
            SUsername = HttpContext.Session.GetString("SUsername");
                        
        }
    }
}
