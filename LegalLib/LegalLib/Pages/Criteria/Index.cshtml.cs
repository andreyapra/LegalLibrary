using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CriteriaIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CriteriaIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IList<tblCriteria> tblCriteria { get; set; }

        public async Task OnGetAsync()
        {
            tblCriteria = await _context.tblCriteria.ToListAsync();
        }
    }
}
