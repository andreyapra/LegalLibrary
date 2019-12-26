using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LegalLib
{
    public class DocCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocCreateModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateCategory();
            PopulateCriteria();
            PopulateRevDocument();
            SUsername = HttpContext.Session.GetString("SUsername");
            SNama = HttpContext.Session.GetString("SNama");
            SEmail = HttpContext.Session.GetString("SEmail");

            return Page();
        }

        public string SUsername { get; set; }
        public string SNama { get; set; }
        public string SEmail { get; set; }
        public SelectList CategorySL { get; set; }
        public SelectList CriteriaSL { get; set; }

        public SelectList RevDocumentSL { get; set; }

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
                           orderby d.CategoryID
                           select d;

            CategorySL = new SelectList(CatQuery, "CategoryID", "Category");

        }

        public void PopulateCriteria()
        {
            var CriQuery = from d in _context.tblCriteria
                           orderby d.CriteriaID
                           select d;

            CriteriaSL = new SelectList(CriQuery, "CriteriaID", "Criteria");
        }
        [BindProperty]
        public tblLegalDocument tblLegalDocument { get; set; }

        public int DocumentID { get; set; }

        public IActionResult AddKlasifikasi()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.tblLegalDocument.Add(tblLegalDocument);
            _context.SaveChanges();

            DocumentID = tblLegalDocument.DocumentID;

            return Redirect("/");
        }
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.tblLegalDocument.Add(tblLegalDocument);
            await _context.SaveChangesAsync();

            DocumentID = tblLegalDocument.DocumentID;
            HttpContext.Session.SetInt32("SDocumentID", DocumentID);

            return Page();
        }
    }
}
