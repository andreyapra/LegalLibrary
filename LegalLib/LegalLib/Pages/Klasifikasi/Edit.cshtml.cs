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
    public class KlasifikasiEditModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }

        public KlasifikasiEditModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public List<TblCategory> TblCategory { get; set; }

        [BindProperty]
        public TblKlasifikasi TblKlasifikasi { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int KlaID { get; set; }

        public async Task LogActivity()
        {
            string Username = HttpContext.Session.GetString("SUsername");
            //Logging Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "KLASIFIKASI";
            TblLogActivity.Action = "EDIT";
            TblLogActivity.Description = "KLASIFIKASIID=" + KlaID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "KLASIFIKASI");
            oJsonObject.Add("action", "EDIT " + "KLASIFIKASIID=" + KlaID);
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

            TblKlasifikasi = await _context.TblKlasifikasi.FirstOrDefaultAsync(m => m.KlasifikasiID == id);

            if (TblKlasifikasi == null)
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
            TblKlasifikasi.ModifiedDate = System.DateTime.Now;
            TblKlasifikasi.ModifiedBy = HttpContext.Session.GetString("SUsername");
            _context.Attach(TblKlasifikasi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblKlasifikasiExists(TblKlasifikasi.KlasifikasiID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            KlaID = TblKlasifikasi.KlasifikasiID;
            await LogActivity();

            return RedirectToPage("./Index");
        }

        private bool tblKlasifikasiExists(int id)
        {
            return _context.TblKlasifikasi.Any(e => e.KlasifikasiID == id);
        }
    }
}
