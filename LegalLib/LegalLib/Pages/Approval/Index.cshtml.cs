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
    public class ApproveIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public ApproveIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public List<tblCriteria> CriteriaList { get; set; }
        public IList<tblLegalDocument> tblLegalDocument { get;set; }
        
        public List<tblLegalDocument> ApprovalList { get; set; }

        public void GetApprovalList()
        {
            var DocQuery = from d in _context.tblLegalDocument
                           where d.ApproveStatus == 0
                           select d;

            ApprovalList = new List<tblLegalDocument>(DocQuery);

        }

        public string GetCriteria(int id)
        {
            var DocQuery = from d in _context.tblCriteria
                           where d.CriteriaID == id
                           select d;

            CriteriaList = new List<tblCriteria>(DocQuery);
            
            string strCriteria = CriteriaList[0].Criteria;

            return strCriteria;  
        }
        public async Task<IActionResult> OnGetAsync()
        {
            tblLegalDocument = await _context.tblLegalDocument.ToListAsync();

            if (tblLegalDocument == null)
            {
                return NotFound();
            }

            GetApprovalList();
            return Page();
        }
    }
}
