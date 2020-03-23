using LegalLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;


namespace LegalLib
{
    public class DocCreateModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocCreateModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int DocID { get; set; }

        public async Task LogActivity()
        {
            string Username = HttpContext.Session.GetString("SUsername");
            //Logging Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "DOCUMENT";
            TblLogActivity.Action = "CREATE";
            TblLogActivity.Description = "DOCUMENTID=" + DocID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = "https://apps.pertamina.com/api/login/LogUsman/InsertLog";
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "DOCUMENT");
            oJsonObject.Add("action", "CREATE " + "DOCUMENTID=" + DocID);
            oJsonObject.Add("appname", "Digital Library");

            var _Client = new HttpClient();
//            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
//            var _content = await _response.Content.ReadAsStringAsync();

        }

        public async Task<ActionResult> OnGet()
        {
            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                return Redirect("/Login");
            }
            if (SRole < 2)
            {
                return Redirect("/Denied");
            }

            int n = _context.TblLegalDocument.Count();
            DocumentID = n + 1;

            TblLegalDocument.DocumentID = n + 1;
            TblLegalDocument.Nomor = DocumentID.ToString();

            TblLegalDocument.UploaderID = HttpContext.Session.GetString("SUsername");
            TblLegalDocument.UploaderName = HttpContext.Session.GetString("SNama");
            TblLegalDocument.UploaderEmail = HttpContext.Session.GetString("SEmail");
            TblLegalDocument.TglUpload = System.DateTime.Now;
            TblLegalDocument.ModifiedDate = System.DateTime.Now;

            TblLegalDocument.TglMulai = System.DateTime.Today;
            TblLegalDocument.TglAkhir = System.DateTime.Today.AddMonths(1);
            TblLegalDocument.Revisi = 0;
            TblLegalDocument.RevDocument = 0;
            TblLegalDocument.PermitDueDate = System.DateTime.Today;
            TblLegalDocument.ReportDueDate = System.DateTime.Today;
            TblLegalDocument.CategoryID = 1;
            TblLegalDocument.CriteriaID = 1;

            TblLegalDocument.ApproveStatus = "0";
            TblLegalDocument.Status = "BARU";
            TblLegalDocument.IsActive = true;

            if (SUsername != null && SRole > 1)
            {
                _context.TblLegalDocument.Add(TblLegalDocument);
                await _context.SaveChangesAsync();

                DocID = TblLegalDocument.DocumentID;
                await LogActivity();

                return Redirect("DocEdit/" + DocumentID);
            }

            return Page();

        }

        public string SUsername { get; set; }
        public int SRole { get; set; }
        public string SEmail { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLegalDocument TblLegalDocument { get; set; }

        public int DocumentID { get; set; }

    }
}
