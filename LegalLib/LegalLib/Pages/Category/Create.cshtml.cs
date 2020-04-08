using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace LegalLib
{
    public class CategoryCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;
        public IConfiguration Configuration { get; }

        public CategoryCreateModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        [BindProperty]
        public string SUsername { get; set; }
        public int SRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int CatID { get; set; }
        public List<TblCategory> TblListCategory { get; set; }


        public async Task LogActivity()
        {
            string Username = HttpContext.Session.GetString("SUsername");
            //Logging Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "CATEGORY";
            TblLogActivity.Action = "CREATE";
            TblLogActivity.Description = "CATEGORYID=" + CatID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "CATEGORY");
            oJsonObject.Add("action", "CREATE " + "CATEGORYID=" + CatID);
            oJsonObject.Add("appname", "Digital Library");

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            _ = await _response.Content.ReadAsStringAsync();

        }


        public async Task<IActionResult> OnGet()
        {
            TblListCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                return RedirectToPage("/Login/Index");
            }
            else if (SRole < 2)
            {
                return RedirectToPage("/Denied");
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

            CatID = TblCategory.CategoryID;
            await LogActivity();
            
            return RedirectToPage("Index");
        }
    }
}
