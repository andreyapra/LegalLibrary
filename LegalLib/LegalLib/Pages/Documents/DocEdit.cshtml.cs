using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    public class DocEditModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocEditModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public TblFileAttach TblFileAttach { get; set; }
        [BindProperty]
        public string Info { get; set; }
        [BindProperty]
        public TblDK TblAddDK { get; set; }
        public TblDK TblDK { get; set; }

        [BindProperty]
        public TblLegalDocument TblLegalDocument { get; set; }
        public SelectList CategorySL { get; set; }
        public SelectList CriteriaSL { get; set; }
        public SelectList KlasifikasiSL { get; set; }
        public SelectList RevDocumentSL { get; set; }

        public string SUsername { get; set; }
        public int SRole { get; set; }

        public void PopulateRevDocument()
        {
            var DocQuery = from d in _context.TblLegalDocument
                           orderby d.DocumentID
                           select d;

            RevDocumentSL = new SelectList(DocQuery, "DocumentID", "NamaDocument");

        }
        public void PopulateCategory()
        {
            var CatQuery = from d in _context.TblCategory
                           where d.IsActive == true
                           orderby d.CategoryID
                           select d;

            CategorySL = new SelectList(CatQuery, "CategoryID", "Category");
        }

        public void PopulateCriteria()
        {
            var CriQuery = from d in _context.TblCriteria
                           where d.IsActive == true
                           orderby d.CriteriaID
                           select d;

            CriteriaSL = new SelectList(CriQuery, "CriteriaID", "Criteria");
        }

        public void PopulateKlasifikasi()
        {
            var CatQuery = from d in _context.TblKlasifikasi
                           where d.IsActive == true
                           orderby d.KlasifikasiID
                           select d;

            KlasifikasiSL = new SelectList(CatQuery, "KlasifikasiID", "Klasifikasi");
        }

        public async Task<IActionResult> OnGetAsync(int? id)
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
                Response.Redirect("/Login");
            }
            else if (SRole < 2)
            {
                Response.Redirect("/Denied");
            }

            PopulateCategory();
            PopulateCriteria();
            PopulateKlasifikasi();
            PopulateRevDocument();

            HttpContext.Session.SetInt32("SID", id.Value);
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostSaveDocument()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TblLegalDocument.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblLegalDocument.ModifiedDate = System.DateTime.Now;
            TblLegalDocument.ApproveStatus = "0";
            TblLegalDocument.IsActive = true;

            _context.Attach(TblLegalDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblLegalDocumentExists(TblLegalDocument.DocumentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddDK()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TblAddDK.DocumentID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
            TblAddDK.IsActive = true;
            TblAddDK.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblAddDK.ModifiedDate = System.DateTime.Now;
            TblAddDK.CreatedBy = HttpContext.Session.GetString("SUsername");
            TblAddDK.CreatedDate = System.DateTime.Now;

            _context.TblDK.Add(TblAddDK);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostFileUpload()
        {
            string rootdir = "D:/BACKUP/uploads/";
            string folder = HttpContext.Session.GetInt32("SID").GetValueOrDefault().ToString();
            var file = Path.Combine(rootdir, folder, Upload.FileName);
            string folderpath = rootdir + folder;

            if (Directory.Exists(folderpath) == false)
            {
                Directory.CreateDirectory(folderpath);
            }

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            Info = file;

            TblFileAttach.DocumentID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
            TblFileAttach.Filename = Upload.FileName;
            TblFileAttach.IsActive = true;
            TblFileAttach.CreatedBy = HttpContext.Session.GetString("SUsername");
            TblFileAttach.CreatedDate = System.DateTime.Now;
            TblFileAttach.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblFileAttach.ModifiedDate = System.DateTime.Now;

            _context.TblFileAttach.Add(TblFileAttach);
            await _context.SaveChangesAsync();

            return RedirectToPage();

        }
        private bool tblLegalDocumentExists(int id)
        {
            return _context.TblLegalDocument.Any(e => e.DocumentID == id);
        }
    }
}
