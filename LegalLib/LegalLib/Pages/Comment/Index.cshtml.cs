using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LegalLib.Data;
using LegalLib.Models;

namespace LegalLib
{
    public class CommentIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public CommentIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<TblComment> TblComment { get;set; }
        [BindProperty]
        public TblComment TblNewComment { get; set; }
        public List<TblCategory> TblCategory { get; set; }

        public string SUsername { get; set; }
        public int SRole { get; set; }


        public int DocumentID { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TblNewComment.DocumentID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
            TblNewComment.CommentDate = DateTime.Today;
            TblNewComment.UserID = HttpContext.Session.GetString("SUsername");
            TblNewComment.Username = HttpContext.Session.GetString("SNama");
            TblNewComment.IsActive = true;

            _context.TblComment.Add(TblNewComment);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index",DocumentID);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                Response.Redirect("/Login/Index");
            }
            else if (SRole < 1)
            {
                Response.Redirect("/Denied");
            }

            DocumentID = id.Value;
            HttpContext.Session.SetInt32("SID", id.Value);

            TblComment = await _context.TblComment.Where(m => m.DocumentID == id.Value).ToListAsync();
            return Page();
        }
    }
}
