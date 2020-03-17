using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class CriteriaCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CriteriaCreateModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public List<TblCategory> TblCategory { get; set; }

        public async Task<IActionResult> OnGet()
        {
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

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
            return Page();
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
            TblLogActivity.Action = "CREATE";
            TblLogActivity.Description = "CRITERIAID=" + CriID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TblCriteria.IsActive = true;
            TblCriteria.CreatedBy = HttpContext.Session.GetString("SUsername");
            TblCriteria.CreatedDate = System.DateTime.Now;
            TblCriteria.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblCriteria.ModifiedDate = System.DateTime.Now;

            _context.TblCriteria.Add(TblCriteria);
            await _context.SaveChangesAsync();

            CriID = TblCriteria.CriteriaID;
            await LogActivity();

            return RedirectToPage("./Index");
        }
    }
}
