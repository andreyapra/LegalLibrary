using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class CriteriaIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }
        public CriteriaIndexModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;

        }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public List<TblCategory> TblCategory { get; set; }

        public IList<TblCriteria> TblCriteria { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var Criteria = from m in _context.TblCriteria
                           select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                Criteria = Criteria.Where(s => s.Criteria.Contains(SearchString));
            }

            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            TblCriteria = await Criteria.Where(t => t.IsActive == true).ToListAsync();

            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                return RedirectToPage("/Login");
            }
            else if (SRole < 2)
            {
                return RedirectToPage("/Denied");
            }
            return Page();
        }
    }
}
