using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace LegalLib
{
    public class DocListModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocListModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SCriteria { get; set; }

        public IList<TblLegalDocument> TblLegalDocument { get; set; }

        public async Task OnGetAsync()
        {
            TblLegalDocument = await _context.TblLegalDocument.ToListAsync();
        }
    }
}
