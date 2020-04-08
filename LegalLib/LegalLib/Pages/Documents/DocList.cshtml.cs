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
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class DocListModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }

        public DocListModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SCriteria { get; set; }
        public string SUsername { get; set; }

        public List<TblCategory> TblCategory { get; set; }

        public IList<TblLegalDocument> TblLegalDocument { get; set; }
        public string GetCategory(int id)
        {
            string Category;
            Category = _context.TblCategory.Where(m => m.CategoryID == id).FirstOrDefault().Category;

            return Category;
        }
        public string GetCriteria(int id)
        {
            string Criteria;
            Criteria = _context.TblCriteria.Where(m => m.CriteriaID == id).FirstOrDefault().Criteria;
            
            return Criteria;
        }

        public string DetectTanggal(string SearchString)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            string tglformat, outtglformat = null;
            DateTime Hasil_yyyy, Hasil_yyyyMM, Hasil_yyyyMMdd, Hasil_MMdd;

            tglformat = "yyyy";
            //Detect format yyyy
            if (DateTime.TryParseExact(SearchString, tglformat, provider, DateTimeStyles.None, out Hasil_yyyy))
            {
                outtglformat = tglformat;
            }
            tglformat = "yyyy-MM";
            //Detect format yyyy-MM
            if (DateTime.TryParseExact(SearchString, tglformat, provider, DateTimeStyles.None, out Hasil_yyyyMM))
            {
                outtglformat = tglformat;
            }
            tglformat = "yyyy-MM-dd";
            //Detect format yyyy-MM
            if (DateTime.TryParseExact(SearchString, tglformat, provider, DateTimeStyles.None, out Hasil_yyyyMMdd))
            {
                outtglformat = tglformat;
            }
            tglformat = "MM-dd";
            //Detect format MM-dd
            if (DateTime.TryParseExact(SearchString, tglformat, provider, DateTimeStyles.None, out Hasil_MMdd))
            {
                outtglformat = tglformat;
            }

            return (outtglformat);
        }

        public void PopulateDocument()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime SDate;
            string DateFmt;

            var DocQuery = from d in _context.TblLegalDocument
//                           where d.ApproveStatus == "APPROVE"
//                           where d.Status != "CABUT"
//                           where d.TglAkhir > System.DateTime.Today
                           where d.IsActive == true
//                           where d.CategoryID == id
                           select d;

            if (!string.IsNullOrEmpty(SearchString))
            {

                switch (SCriteria)
                {
                    case "NOMOR":
                        DocQuery = DocQuery.Where(s => s.Nomor.Contains(SearchString));
                        break;

                    case "PERIHAL":
                        DocQuery = DocQuery.Where(s => s.Perihal.Contains(SearchString));
                        break;

                    case "TANGGAL":
                        DateFmt = DetectTanggal(SearchString);
                        SDate = DateTime.ParseExact(SearchString,DateFmt,provider,DateTimeStyles.None);

                        switch (DateFmt)
                        {
                            case "yyyy":
                                DocQuery = DocQuery.Where(s => s.TglMulai.Year == SDate.Year);
                                break;

                            case "yyyy-MM":
                                DocQuery = DocQuery.Where(s => s.TglMulai.Year == SDate.Year).Where(s => s.TglMulai.Month == SDate.Month);
                                break;

                            case "yyyy-MM-dd":
                                DocQuery = DocQuery.Where(s => s.TglMulai.Year == SDate.Year).Where(s => s.TglMulai.Month == SDate.Month).Where(s => s.TglMulai.Day == SDate.Day);
                                break;

                            case "MM-dd":
                                DocQuery = DocQuery.Where(s => s.TglMulai.Year == SDate.Year).Where(s => s.TglMulai.Month == SDate.Month).Where(s => s.TglMulai.Day == SDate.Day);
                                break;

                        }
                        break;

                    case "TAKHIR":
                        DateFmt = DetectTanggal(SearchString);
                        SDate = DateTime.ParseExact(SearchString, DateFmt, provider, DateTimeStyles.None);

                        switch (DateFmt)
                        {
                            case "yyyy":
                                DocQuery = DocQuery.Where(s => s.TglAkhir.Year == SDate.Year);
                                break;

                            case "yyyy-MM":
                                DocQuery = DocQuery.Where(s => s.TglAkhir.Year == SDate.Year).Where(s => s.TglAkhir.Month == SDate.Month);
                                break;

                            case "yyyy-MM-dd":
                                DocQuery = DocQuery.Where(s => s.TglAkhir.Year == SDate.Year).Where(s => s.TglAkhir.Month == SDate.Month).Where(s => s.TglAkhir.Day == SDate.Day);
                                break;

                            case "MM-dd":
                                DocQuery = DocQuery.Where(s => s.TglAkhir.Year == SDate.Year).Where(s => s.TglAkhir.Month == SDate.Month).Where(s => s.TglAkhir.Day == SDate.Day);
                                break;

                        }
                        break;

                    case "CRITERIA":
                        var Doc1 = from t1 in _context.TblLegalDocument
                                   join t2 in _context.TblCriteria
                                   on t1.CriteriaID equals t2.CriteriaID
                                   where t2.Criteria.Contains(SearchString)

//                                   where t1.ApproveStatus == "APPROVE"
//                                   where t1.Status != "CABUT"
//                                   where t1.TglAkhir > System.DateTime.Today
                                   where t1.IsActive == true
//                                   where t1.CategoryID == id

                                   select t1;
                        DocQuery = Doc1;
                        break;

                    case "KLASIFIKASI":
                        var Doc2 = (from t1 in _context.TblKlasifikasi
                                    join t2 in _context.TblDK
                                    on t1.KlasifikasiID equals t2.KlasifikasiID
                                    join t3 in _context.TblLegalDocument
                                    on t2.DocumentID equals t3.DocumentID

                                    where t1.Klasifikasi.Contains(SearchString)
//                                    where t3.ApproveStatus == "APPROVE"
//                                    where t3.Status != "CABUT"
//                                    where t3.TglAkhir > System.DateTime.Today
                                    where t3.IsActive == true
//                                    where t3.CategoryID == id

                                    select t3).Distinct();
                        DocQuery = Doc2;
                        break;
                }
            }

            TblLegalDocument = DocQuery.ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();
            SUsername = HttpContext.Session.GetString("SUsername");
            PopulateDocument();

            return Page();

        }
    }
}
