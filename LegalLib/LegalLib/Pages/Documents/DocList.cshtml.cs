using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class DocListModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocListModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IList<tblLegalDocument> tblLegalDocument { get; set; }

        public async Task OnGetAsync()
        {
            tblLegalDocument = await _context.tblLegalDocument.ToListAsync();
        }
    }
}
