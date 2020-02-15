using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LegalLib.Data;
using LegalLib.Models;


namespace LegalLib
{
    public class SearchIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public SearchIndexModel(LegalLib.Data.LegalLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IList<TblLegalDocument> TblDocSearch { get; set; }
        public List<TblDK> TblDocK { get; set; }

        [BindProperty]
        public string SearchNomor { get; set; }
        [BindProperty]
        public DateTime SearchTglStart { get; set; }
        [BindProperty]
        public int SearchCriteriaID { get; set; }
        [BindProperty]
        public string SearchPerihal { get; set; }
        [BindProperty]
        public int SearchKlasifikasiID { get; set; }
        [BindProperty]
        public string Search { get; set; }
        public SelectList CriteriaSL { get; set; }
        public SelectList KlasifikasiSL { get; set; }   
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
            var CriQuery = from d in _context.TblKlasifikasi
                           where d.IsActive == true
                           orderby d.KlasifikasiID
                           select d;

            KlasifikasiSL = new SelectList(CriQuery, "KlasifikasiID", "Klasifikasi");
        }
        public void PopulateDK()
        {
            var DKQuery = from d in _context.TblDK
                          select d;

            TblDocK = DKQuery.ToList();
        }
        public string GetKlasifikasi(int id)
        {
            string Klasifikasi;
            Klasifikasi = _context.TblKlasifikasi.Where(m => m.KlasifikasiID == id).FirstOrDefault().Klasifikasi;

            return Klasifikasi;
        }

        public string GetCriteria(int id)
        {
            string Criteria;
            Criteria = _context.TblCriteria.Where(m => m.CriteriaID == id).FirstOrDefault().Criteria;

            return Criteria;
        }
        public string GetCategory(int id)
        {
            string Category;
            Category = _context.TblCategory.Where(m => m.CategoryID == id).FirstOrDefault().Category;

            return Category;
        }


        public void OnGet()
        {
            PopulateCriteria();
            PopulateKlasifikasi();
            PopulateDK();
            TblDocSearch = _context.TblLegalDocument.ToList();
        }

        public IActionResult OnPost()
        {
            Search = null;

            if (SearchCriteriaID != 0)
            {
                Search += "Criteria";
            }
            if (SearchKlasifikasiID != 0)
            {
                Search += "Klasifikasi";
                // cari di tbldk, terus cari tbllegaldoc
            }
            if (SearchNomor != null)
            {
                Search += "Nomor";
            }
            if (SearchPerihal != null)
            {
                Search += "Perihal";
            }
            if (SearchTglStart != DateTime.MinValue)
            {
                Search += "TglStart";
            }

            PopulateCriteria();
            PopulateKlasifikasi();
            PopulateDK();
            return Page();
        }
    }
}