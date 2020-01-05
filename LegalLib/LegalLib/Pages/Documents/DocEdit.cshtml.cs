using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LegalLib.Data;
using LegalLib.Models;

namespace LegalLib
{
    public class DocEditModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocEditModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public tblLegalDocument tblLegalDocument { get; set; }
        [BindProperty]
        public int SelectedCategory { get; set; }
        public SelectList CategorySL { get; set; }
        public SelectList CriteriaSL { get; set; }

        public SelectList RevDocumentSL { get; set; }

        public int DocumentID { get; set; }
        public void PopulateRevDocument()
        {
            var DocQuery = from d in _context.tblLegalDocument
                           orderby d.DocumentID
                           select d;

            RevDocumentSL = new SelectList(DocQuery, "DocumentID", "NamaDocument");

        }
        public void PopulateCategory()
        {
            var CatQuery = from d in _context.tblCategory
                           where d.IsActive == true
                           orderby d.CategoryID
                           select d;

            CategorySL = new SelectList(CatQuery, "CategoryID", "Category");
            SelectedCategory = tblLegalDocument.CategoryID;
        }

        public void PopulateCriteria()
        {
            var CriQuery = from d in _context.tblCriteria
                           where d.IsActive == true
                           orderby d.CriteriaID
                           select d;

            CriteriaSL = new SelectList(CriQuery, "CriteriaID", "Criteria");
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentID = id.Value;
            tblLegalDocument = await _context.tblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);

            if (tblLegalDocument == null)
            {
                return NotFound();
            }

            PopulateCategory();
            PopulateCriteria();
            PopulateRevDocument();
            HttpContext.Session.SetInt32("sDocumentID", id.GetValueOrDefault());

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            tblLegalDocument.ApproveStatus = 0;
            _context.Attach(tblLegalDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblLegalDocumentExists(tblLegalDocument.DocumentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool tblLegalDocumentExists(int id)
        {
            return _context.tblLegalDocument.Any(e => e.DocumentID == id);
        }
    }
}
