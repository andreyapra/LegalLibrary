using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LegalLib
{
    public class CategoryCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CategoryCreateModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string SUsername { get; set; }
        public int SRole { get; set; }


        public IActionResult OnGet()
        {
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
        public TblCategory TblCategory { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            TblCategory.IsActive = true;
            TblCategory.CreatedBy = HttpContext.Session.GetString("SUsername");
            TblCategory.CreatedDate = System.DateTime.Now;
            TblCategory.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblCategory.ModifiedDate = System.DateTime.Now;

            _context.TblCategory.Add(TblCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
