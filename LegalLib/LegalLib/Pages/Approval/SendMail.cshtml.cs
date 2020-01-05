using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LegalLib
{
    public class SendMailModel : PageModel
    {
        public async Task<IActionResult> SendMail()
        {
            var body = $@"<p>Test Kirim Email</p>";

            string Pengirim = "pengirim@email.com";

            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                smtp.PickupDirectoryLocation = @"D:\BACKUP";
                var message = new MailMessage();
                message.To.Add(Pengirim);
                message.Subject = "Fourth Coffee - New Order";
                message.Body = body;
                message.IsBodyHtml = true;
                message.From = new MailAddress("sales@fourthcoffee.com");
                await smtp.SendMailAsync(message);
            }
            return Page();
        }
        
            public async void OnGet()
        {
            await SendMail();
        }
    }
}