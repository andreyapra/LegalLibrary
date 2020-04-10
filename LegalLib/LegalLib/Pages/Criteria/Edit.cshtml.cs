using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class CriteriaEditModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }

        public CriteriaEditModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [BindProperty]
        public TblCriteria TblCriteria { get; set; }
        public List<TblCategory> TblCategory { get; set; }

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
            TblLogActivity.Action = "EDIT";
            TblLogActivity.Description = "CRITERIAID=" + CriID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "CRITERIA");
            oJsonObject.Add("action", "EDIT " + "CRITERIAID=" + CriID);
            oJsonObject.Add("appname", "Online Library");

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
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            TblCriteria = await _context.TblCriteria.FirstOrDefaultAsync(m => m.CriteriaID == id);

            if (TblCriteria == null)
            {
                return NotFound();
            }
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
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
