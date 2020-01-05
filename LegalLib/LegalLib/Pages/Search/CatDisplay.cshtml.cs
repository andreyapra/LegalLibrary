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
        public List<tblDK> tblDocK { get; set; }

        public void PopulateDocument(int id)
        {
            var DocQuery = from d in _context.tblLegalDocument
                            where d.ApproveStatus == 1
                            where d.Status != 2
                            where d.TglAkhir > System.DateTime.Today
                            where d.CategoryID == id
                            select d;

            tblLegalDocument = new List<tblLegalDocument>(DocQuery);
        }
        public void PopulateDK()
        {
            var KQuery = from d in _context.tblDK
                         select d;

            tblDocK = new List<tblDK>(KQuery);

        }

        public string GetKlasifikasi(int id)
        {
            string Klasifikasi;
            Klasifikasi = _context.tblKlasifikasi.Where(m => m.KlasifikasiID == id).FirstOrDefault().Klasifikasi;

            return Klasifikasi;
        }

        public string GetCriteria(int id)
        {
            string Criteria;
            Criteria = _context.tblCriteria.Where(m => m.CriteriaID == id).FirstOrDefault().Criteria;

            return Criteria;
        }
        public string GetCategory(int id)
        {
            string Category;
            Category = _context.tblCategory.Where(m => m.CategoryID == id).FirstOrDefault().Category;

            return Category;
        }

        public IActionResult OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PopulateDocument(id.Value);
            PopulateDK();

            return Page();
        }
    }
}
