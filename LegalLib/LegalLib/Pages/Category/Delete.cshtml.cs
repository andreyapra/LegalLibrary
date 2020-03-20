using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;


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
            string Username = HttpContext.Session.GetString("SUsername");
            //Logging Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "CATEGORY";
            TblLogActivity.Action = "DELETE";
            TblLogActivity.Description = "CATEGORYID=" + CatID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = "https://apps.pertamina.com/api/login/LogUsman/InsertLog";
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "CATEGORY");
            oJsonObject.Add("action", "DELETE " + "CATEGORYID=" + CatID);
            oJsonObject.Add("appname", "Digital Library");

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            var _content = await _response.Content.ReadAsStringAsync();

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
