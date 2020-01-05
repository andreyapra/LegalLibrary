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
    public class CommentIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CommentIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IList<tblComments> tblComments { get;set; }

        public int DocumentID { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentID = id.Value;

            tblComments = await _context.tblComments.Where(m => m.DocumentID == id).ToListAsync();
            return Page();
        }
    }
}
