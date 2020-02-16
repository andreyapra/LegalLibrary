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
        public string GetCategory(int id)
        {
            string Category;
            Category = _context.TblCategory.Where(m => m.CategoryID == id).FirstOrDefault().Category;

            return Category;
        }
        public string GetCriteria(int id)
        {
            string Criteria;
            Criteria = _context.TblCriteria.Where(m => m.CriteriaID == id).FirstOrDefault().Criteria;

            return Criteria;
        }

        public async Task OnGetAsync()
        {
            TblLegalDocument = await _context.TblLegalDocument.ToListAsync();
        }
    }
}
