using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using LegalLib.Models;
using Microsoft.EntityFrameworkCore;

namespace LegalLib
{
    public class DocUploadModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public DocUploadModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public tblFileAttach tblFileAttach { get; set; }
        [BindProperty]
        public int DocumentID { get; set; }
        public string Filename { get; set; }
        public string Info { get; set; }

        public async Task OnPostAsync()
        {
            string rootdir = "D:/BACKUP/uploads/";
            string folder = DocumentID.ToString();
            
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

            tblFileAttach.DocumentID = DocumentID;
            tblFileAttach.Filename = Upload.FileName;

            _context.tblFileAttach.Add(tblFileAttach);
            await _context.SaveChangesAsync();

        }

        public IActionResult OnGetAsync(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            DocumentID = id.Value;
            
            return Page();
        }
    }
}