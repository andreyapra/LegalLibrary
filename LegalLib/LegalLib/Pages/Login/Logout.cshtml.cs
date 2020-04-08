using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LegalLib.Data;
using LegalLib.Models;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class LogoutIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }

        public LogoutIndexModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [BindProperty(SupportsGet =true)]
        public TblLogActivity TblLogActivity { get; set; }
        [BindProperty]
        public string Username { get; set; }

        public async Task LogActivity()
        {
            //Logging Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "LOGIN";
            TblLogActivity.Action = "LOGOUT";
            TblLogActivity.Description = "USER=" + Username;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject
            {
                { "username", Username },
                { "modul", "LOGIN" },
                { "action", "LOGOUT " + "USER=" + Username },
                { "appname", "Digital Library" }
            };

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            _ = await _response.Content.ReadAsStringAsync();

        }

        public async Task<IActionResult> OnGet()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Username = HttpContext.Session.GetString("SUsername");

            if (Username != null)
            {
                await LogActivity();
                HttpContext.Session.Clear();

                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }

        }
    }
}