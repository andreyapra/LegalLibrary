using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace LegalLib
{
    public class CategoryDeleteModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CategoryDeleteModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TblCategory TblCategory { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int CatID { get; set; }

        public async Task LogActivity()
        {
            TblLogActivity.UserID = HttpContext.Session.GetString("SUsername");
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "CATEGORY";
            TblLogActivity.Action = "DELETE";
            TblLogActivity.Description = "CATEGORYID=" + CatID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblCategory = await _context.TblCategory.FirstOrDefaultAsync(m => m.CategoryID == id);

            if (TblCategory == null)
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

            TblCategory.IsActive = false;
            TblCategory.ModifiedDate = System.DateTime.Now;
            TblCategory.ModifiedBy = HttpContext.Session.GetString("SUsername");
            _context.Attach(TblCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblCategoryExists(TblCategory.CategoryID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            CatID = TblCategory.CategoryID;
            await LogActivity();

            return RedirectToPage("./Index");
        }

        private bool tblCategoryExists(int id)
        {
            return _context.TblCategory.Any(e => e.CategoryID == id);
        }

    }
}
