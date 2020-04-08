using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class ApproveIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;
        public IConfiguration Configuration { get; }

        public ApproveIndexModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string SUsername { get; set; }
        public int SRole { get; set; }

        public List<TblLegalDocument> TblLegalDocument { get;set; }
        
        public List<TblDK> TblDKs { get; set; }

        public List<TblCategory> TblCategory { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            TblLegalDocument = await _context.TblLegalDocument.Where(m => m.ApproveStatus == "0").ToListAsync();

            if (TblLegalDocument == null)
            {
                return NotFound();
            }
            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                return RedirectToPage("/Login");
            }
            else if (SRole < 3)
            {
                return RedirectToPage("/Denied");
            }

            return Page();
        }
    }
}
