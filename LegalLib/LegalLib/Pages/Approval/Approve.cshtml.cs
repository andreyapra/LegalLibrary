using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class ApproveModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public ApproveModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public tblLegalDocument tblLegalDocument { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            tblLegalDocument = await _context.tblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);

            if (tblLegalDocument == null)
            {
                return NotFound();
            }

            tblLegalDocument.ApproveStatus = 1;
            _context.Attach(tblLegalDocument).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Redirect("Index");

        }
    }
}