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
    public class CriteriaCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }

        public CriteriaCreateModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public List<TblCategory> TblCategory { get; set; }

        public async Task<IActionResult> OnGet()
        {
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

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
        public TblCriteria TblCriteria { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int CriID { get; set; }

        public async Task LogActivity()
        {
            string Username = HttpContext.Session.GetString("SUsername");
            //Logging Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "CRITERA";
            TblLogActivity.Action = "CREATE";
            TblLogActivity.Description = "CRITERIAID=" + CriID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "CRITERIA");
            oJsonObject.Add("action", "CREATE " + "CRITERIAID=" + CriID);
            oJsonObject.Add("appname", "Online Library");

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            var _content = await _response.Content.ReadAsStringAsync();

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
