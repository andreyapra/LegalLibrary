using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LegalLib.Data;
using LegalLib.Models;

namespace LegalLib
{
    public class CommentCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CommentCreateModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public tblComments tblComments { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.tblComments.Add(tblComments);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
