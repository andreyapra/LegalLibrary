﻿using LegalLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LegalLib
{
    public class KlasifikasiEditModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public KlasifikasiEditModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        public List<TblCategory> TblCategory { get; set; }

        [BindProperty]
        public TblKlasifikasi TblKlasifikasi { get; set; }
        public string SUsername { get; set; }
        public int SRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int KlaID { get; set; }

        public async Task LogActivity()
        {
            TblLogActivity.UserID = HttpContext.Session.GetString("SUsername");
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "KLASIFIKASI";
            TblLogActivity.Action = "EDIT";
            TblLogActivity.Description = "KLASIFIKASIID=" + KlaID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            TblKlasifikasi = await _context.TblKlasifikasi.FirstOrDefaultAsync(m => m.KlasifikasiID == id);

            if (TblKlasifikasi == null)
            {
                return NotFound();
            }
            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                Response.Redirect("/Login/Index");
            }
            else if (SRole < 2)
            {
                Response.Redirect("/Denied");
            }

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            TblKlasifikasi.ModifiedDate = System.DateTime.Now;
            TblKlasifikasi.ModifiedBy = HttpContext.Session.GetString("SUsername");
            _context.Attach(TblKlasifikasi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblKlasifikasiExists(TblKlasifikasi.KlasifikasiID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            KlaID = TblKlasifikasi.KlasifikasiID;
            await LogActivity();

            return RedirectToPage("./Index");
        }

        private bool tblKlasifikasiExists(int id)
        {
            return _context.TblKlasifikasi.Any(e => e.KlasifikasiID == id);
        }
    }
}
