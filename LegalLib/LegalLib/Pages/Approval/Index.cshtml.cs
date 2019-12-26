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

        public IList<tblLegalDocument> tblLegalDocument { get;set; }
        
        public List<tblLegalDocument> ApprovalList { get; set; }

        public void GetApprovalList()
        {
            var DocQuery = from d in _context.tblLegalDocument
                           where d.ApproveStatus == 0
                           select d;

            ApprovalList = new List<tblLegalDocument>(DocQuery);

        }
        public async Task OnGetAsync()
        {
            tblLegalDocument = await _context.tblLegalDocument.ToListAsync();

            GetApprovalList();

        }
    }
}
