using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CriteriaDeleteModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CriteriaDeleteModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblCriteria TblCriteria { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int CriID { get; set; }

        public async Task LogActivity()
        {
            TblLogActivity.UserID = HttpContext.Session.GetString("SUsername");
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "CRITERA";
            TblLogActivity.Action = "DELETE";
            TblLogActivity.Description = "CRITERIAID=" + CriID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblCriteria = await _context.TblCriteria.FirstOrDefaultAsync(m => m.CriteriaID == id);

            if (TblCriteria == null)
            {
                return NotFound();
            }
            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                Response.Redirect("/Login/Index");
            }
            else if (SRole < 2)
            {
                Response.Redirect("/Denied");
            }

            TblCriteria.IsActive = false;
            TblCriteria.ModifiedDate = System.DateTime.Now;
            TblCriteria.ModifiedBy = HttpContext.Session.GetString("SUsername");
            _context.Attach(TblCriteria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblCriteriaExists(TblCriteria.CriteriaID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            CriID = TblCriteria.CriteriaID;
            await LogActivity();

            return RedirectToPage("./Index");
        }

        private bool tblCriteriaExists(int id)
        {
            return _context.TblCriteria.Any(e => e.CriteriaID == id);
        }

    }
}
