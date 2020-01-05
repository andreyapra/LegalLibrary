using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace LegalLib
{
    public class MenuModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public MenuModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<tblCategory> tblCategory { get; set; }
        [BindProperty]
        public List<tblLegalDocument> tblLegalDocument { get; set; }
        public string SUsername { get; set; }

        public void PopulateDocument()
        {
            var DocQuery = (from d in _context.tblLegalDocument
                           orderby d.TglMulai descending
                           select d).Take(10);

            tblLegalDocument = new List<tblLegalDocument>(DocQuery);
        }

        public async Task OnGetAsync()
        {

            tblCategory = await _context.tblCategory.Where(m => m.IsActive == true).ToListAsync();

            PopulateDocument();
            SUsername = HttpContext.Session.GetString("SUsername");
                        
        }
    }
}
