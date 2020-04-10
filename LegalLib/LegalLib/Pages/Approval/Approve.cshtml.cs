using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class ApproveModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;
        public IConfiguration Configuration { get; }

        public ApproveModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public TblLegalDocument TblLegalDocument { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int DocID { get; set; }


        public async void SendMailApprove()
        {
            //Fungsi Send Email
            var StrBody = $@"<p>Dokumen Nomor " + TblLegalDocument.Nomor + " di Approve</p>";
            var StrSubject = "Dokumen Nomor " + TblLegalDocument.Nomor + " di Approve";
            string StrMailto = TblLegalDocument.UploaderEmail;

            using var smtp = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = @"D:\BACKUP\EMAIL"
            };
            var message = new MailMessage();
            message.To.Add(StrMailto);
            message.Subject = StrSubject;
            message.Body = StrBody;
            message.IsBodyHtml = true;
            message.From = new MailAddress("digitallibrary@pertamina.com");
            await smtp.SendMailAsync(message);

            //Fungsi Send Email API
            string Baseurl = "http://localhost:8081/CodeIgniter/index.php/email";
            //string Baseurl = "http://10.3.30.61/APILibrary/api/Email";
            //string Baseurl = "http://pepcappprod.pertamina.com/APILibrary/api/email";
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("subject", StrSubject);
            oJsonObject.Add("body", StrBody);
            oJsonObject.Add("mailto", StrMailto);
            oJsonObject.Add("cc", "");
            oJsonObject.Add("bcc", "");

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            var _content = await _response.Content.ReadAsStringAsync();

        }
        public async Task LogActivity()
        {
            string Username = HttpContext.Session.GetString("SUsername");
            //Loggin Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "APPROVAL";
            TblLogActivity.Action = "APPROVE";
            TblLogActivity.Description = "DOCUMENTID=" + DocID ;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "APPROVAL");
            oJsonObject.Add("action", "APPROVE " + "DOCUMENTID=" + DocID);
            oJsonObject.Add("appname", "Online Library");

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            var _content = await _response.Content.ReadAsStringAsync();

        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblLegalDocument = await _context.TblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);

            if (TblLegalDocument == null)
            {
                return NotFound();
            }
            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                Response.Redirect("Login");
            }
            if (SRole < 3)
            {
                Response.Redirect("Denied");
            }
            else
            {
                TblLegalDocument.ApproveStatus = "APPROVE";
                _context.Attach(TblLegalDocument).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                SendMailApprove();

                DocID = id.Value;
                await LogActivity();

            }

            return Redirect("/");

        }
    }
}