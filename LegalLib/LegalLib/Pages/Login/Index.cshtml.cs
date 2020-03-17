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
    public class LoginIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public LoginIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public TblLogActivity TblLogActivity { get; set; }

        public string SUsername { get; set; }
        public List<TblCategory> TblCategory { get; set; }

        public string Message { get; set; }


        public async Task LogActivity()
        {
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "LOGIN";
            TblLogActivity.Action = "LOGIN";
            TblLogActivity.Description = "USER=" + Username;

            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> OnGet()
        {
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            switch (Username) 
            {
                case "manager":
                    HttpContext.Session.SetInt32("SRole", 3);
                    HttpContext.Session.SetString("SNama", "Nama Manager");
                    HttpContext.Session.SetString("SEmail", "manager@email.com");
                    HttpContext.Session.SetString("SUsername", Username);
                    Message = HttpContext.Session.GetString("SUsername");
                    await LogActivity();
                    return RedirectToPage("/Index");

                case "legal":
                    HttpContext.Session.SetInt32("SRole", 2);
                    HttpContext.Session.SetString("SNama", "Nama Legal");
                    HttpContext.Session.SetString("SEmail", "legal@email.com");
                    HttpContext.Session.SetString("SUsername", Username);
                    Message = HttpContext.Session.GetString("SUsername");
                    await LogActivity();
                    return RedirectToPage("/Index");

                case "user":
                    HttpContext.Session.SetInt32("SRole", 1);
                    HttpContext.Session.SetString("SNama", "Nama User");
                    HttpContext.Session.SetString("SEmail", "user@email.com");
                    HttpContext.Session.SetString("SUsername", Username);
                    Message = HttpContext.Session.GetString("SUsername");
                    await LogActivity();
                    return RedirectToPage("/Index");

                default:
                    Message = "Invalid username and password";
                    return Page();
            }
        }
    }
}