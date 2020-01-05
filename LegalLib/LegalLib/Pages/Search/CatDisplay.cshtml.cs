using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LegalLib.Data;
using LegalLib.Models;

namespace LegalLib
{
    public class CatDisplayModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CatDisplayModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IList<tblLegalDocument> tblLegalDocument { get;set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblLegalDocument = await _context.tblLegalDocument.Where(m => m.CategoryID == id).ToListAsync();

            return Page();
        }
    }
}
