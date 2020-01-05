using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


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
        public string SearchNomor { get; set; }
        [BindProperty]
        public DateTime SearchTglStart { get; set; }
        [BindProperty]
        public int SearchCriteriaID { get; set; }
        [BindProperty]
        public string SearchPerihal { get; set; }
        [BindProperty]
        public int SearchKlasifikasiID { get; set; }

        public string Search { get; set; }
        public SelectList CriteriaSL { get; set; }
        public SelectList KlasifikasiSL { get; set; }   
        public void PopulateCriteria()
        {
            var CriQuery = from d in _context.tblCriteria
                           where d.IsActive == true
                           orderby d.CriteriaID
                           select d;

            CriteriaSL = new SelectList(CriQuery, "CriteriaID", "Criteria");
        }
        public void PopulateKlasifikasi()
        {
            var CriQuery = from d in _context.tblKlasifikasi
                           where d.IsActive == true
                           orderby d.KlasifikasiID
                           select d;

            KlasifikasiSL = new SelectList(CriQuery, "KlasifikasiID", "Klasifikasi");
        }


        public void OnGet()
        {
            PopulateCriteria();
            PopulateKlasifikasi();
        }

        public IActionResult OnPost()
        {
            Search = null;

            if (SearchCriteriaID != 0)
            {
                Search = Search + "Criteria";
            }
            if (SearchKlasifikasiID != 0)
            {
                Search = Search + "Klasifikasi";
            }
            if (SearchNomor != null)
            {
                Search = Search + "Nomor";
            }
            if (SearchPerihal != null)
            {
                Search = Search + "Perihal";
            }
            if (SearchTglStart != DateTime.MinValue)
            {
                Search = Search + "TglStart";
            }

            PopulateCriteria();
            PopulateKlasifikasi();
            return Page();
        }
    }
}