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
    public class DocFileListModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocFileListModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public int DocumentID { get; set; }
        public IList<tblFileAttach> tblFileAttach { get;set; }

        public IActionResult OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentID = id.Value;
            var DocQuery = from d in _context.tblFileAttach
                           where d.DocumentID == DocumentID
                           select d;

            tblFileAttach = new List<tblFileAttach>(DocQuery);

            return Page();
        }
    }
}
