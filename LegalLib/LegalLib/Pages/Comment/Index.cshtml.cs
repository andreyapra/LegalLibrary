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
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class CommentIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }

        public CommentIndexModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        [BindProperty]
        public List<TblComment> TblComment { get;set; }
        [BindProperty]
        public TblComment TblNewComment { get; set; }
        public List<TblCategory> TblCategory { get; set; }

        public string SUsername { get; set; }
        public int SRole { get; set; }


        public int DocumentID { get; set; }

        public string GetDocument(int id)
        {
            string Document;
            Document = _context.TblLegalDocument.Where(m => m.DocumentID == id).FirstOrDefault().NamaDocument;

            return Document;
        }

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
                return RedirectToPage("/Login/Index");
            }
            else if (SRole < 1)
            {
                return RedirectToPage("/Denied");
            }

            DocumentID = id.Value;
            HttpContext.Session.SetInt32("SID", id.Value);

            TblComment = await _context.TblComment.Where(m => m.DocumentID == id.Value).ToListAsync();
            return Page();
        }
    }
}
