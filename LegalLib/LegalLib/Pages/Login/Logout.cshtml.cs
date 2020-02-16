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

namespace LegalLib
{
    public class LogoutIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public LogoutIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet =true)]
        public TblLogActivity TblLogActivity { get; set; }
        [BindProperty]
        public string Username { get; set; }

        public async Task LogActivity()
        {
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "LOGIN";
            TblLogActivity.Action = "LOGOUT";
            TblLogActivity.Description = "USER=" + Username;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();
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