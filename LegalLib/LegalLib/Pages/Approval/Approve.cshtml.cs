using LegalLib.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace LegalLib
{
    public class ApproveModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public ApproveModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public TblLegalDocument TblLegalDocument { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int DocID { get; set; }


        public async void SendMailApprove()
        {
            var body = $@"<p>Dokumen Nomor " + TblLegalDocument.Nomor + " di Approve</p>";

            string Kepada = TblLegalDocument.UploaderEmail;

            using var smtp = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = @"D:\BACKUP"
            };
            var message = new MailMessage();
            message.To.Add(Kepada);
            message.Subject = "Dokumen Nomor " + TblLegalDocument.Nomor + " di Approve";
            message.Body = body;
            message.IsBodyHtml = true;
            message.From = new MailAddress("library@pertamina.com");
            await smtp.SendMailAsync(message);


        }
        public async Task LogActivity()
        {
            TblLogActivity.UserID = HttpContext.Session.GetString("SUsername");
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "APPROVAL";
            TblLogActivity.Action = "APPROVE";
            TblLogActivity.Description = "DOCUMENTID=" + DocID ;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();
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