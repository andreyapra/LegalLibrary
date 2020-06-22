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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace LegalLib
{
    public class ArchiveModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }

        public ArchiveModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        //Class Dari USMAN
        public static RootObject _login;
        public class RootObject
        {
            public DataLDAP dataLDAP { get; set; }
            public List<DataSAP> dataSAP { get; set; }
            public DataUsman DataUsman { get; set; }
        }
        public class DataLDAP
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public Data Data { get; set; }
        }
        public class DataSAP
        {
            public string USERNAME { get; set; }
            public string EMPLOYEE_NOPEK { get; set; }
            public string EMPLOYEE_NAMA { get; set; }
            public string EMPLOYEE_POSIDI { get; set; }
            public string EMPLOYEE_POSTEXT { get; set; }
            public string EMPLOYEE_CC { get; set; }
            public string EMPLOYEE_LAYER { get; set; }
            public string EMPLOYEESUBGROUP { get; set; }
            public string EMPLOYEE_EMAIL { get; set; }
            public string GENDER { get; set; }
            public int ISCHIEF { get; set; }
            public string ATASAN_USERNAME { get; set; }
            public string ATASAN_NOPEK { get; set; }
            public string ATASAN_POSIDI { get; set; }
            public string ATASAN_EMAIL { get; set; }
            public string ATASAN_LAYER { get; set; }
            public string ATASAN_EMPLOYEESUBGROUP { get; set; }
            public string COSTCENTERNAME { get; set; }
        }
        public class DataUsman
        {
            public string UserName { get; set; }
            public object AppName { get; set; }
            public List<DataMenu> DataMenu { get; set; }
            public List<DataRoleGroup> DataRoleGroup { get; set; }
            public List<DataRole> DataRole { get; set; }
            public List<DataParameter> DataParameter { get; set; }
        }
        public class Data
        {
            public bool value { get; set; }
            public string EmpNumber { get; set; }
            public string NamaLengkap { get; set; }
            public string Email { get; set; }
            public string Company { get; set; }
            public string Department { get; set; }
            public string Description { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Country { get; set; }
            public string OUmanager { get; set; }
            public string OfficeName { get; set; }
            public string UserName { get; set; }
            public string StreetAddress { get; set; }
            public string TelephoneNumber { get; set; }
            public string Title { get; set; }
            public string OUpekerja { get; set; }
        }
        public class DataMenu
        {
            public string idMenu { get; set; }
            public string ParentID { get; set; }
            public string NoOrder { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string FontClass { get; set; }
        }
        public class DataRoleGroup
        {
            public string idRoleGroup { get; set; }
            public string RoleGroupName { get; set; }
            public string RoleGroupDesc { get; set; }
            public string AppID { get; set; }
            public string AppDesc { get; set; }
        }
        public class DataRole
        {
            public string idRoleGroup { get; set; }
            public string idRole { get; set; }
            public string RoleName { get; set; }
            public string RoleDesc { get; set; }
            public string AppID { get; set; }
            public string AppDesc { get; set; }
        }
        public class DataParameter
        {
            public string IdParameter { get; set; }
            public string ParameterName { get; set; }
            public string ParameterDesc { get; set; }
            public string IdValue { get; set; }
            public string ValueName { get; set; }
        }

        //Akhir Class USMAN

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SCriteria { get; set; }
        public string SUsername { get; set; }
        public string SPassword { get; set; }

        public List<TblCategory> TblCategory { get; set; }

        public IList<TblLegalDocument> TblLegalDocument { get; set; }

        public int CheckRoleName(string rolename)
        {
            return _login.DataUsman.DataRole.Where(m => m.RoleName == rolename).Count();
        }
        public async Task AmbilDataRole()
        {
            //Panggil USMAN
            string Baseurl = Configuration["Setting:UsmanURL"];
            //string Baseurl = "https://apps.pertamina.com/api/login/Users/Loginldap";
            string sContentType = "application/json"; // or application/xml

            JObject oJsonObject = new JObject
            {
                { "username", SUsername },
                { "password", SPassword },
                { "appname", "Online_Library"}
            };

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));

            var _content = await _response.Content.ReadAsStringAsync();
            JObject _item = JObject.Parse(_content);
            var _result = _item.Descendants()
                .OfType<JProperty>()
                .Where(p => p.Name.ToString() == "Status")
                .First();

            if (_result.Value != null)
            {
                if (_result.Value.ToString() == "00")
                {
                    _login = new RootObject();
                    _login = JsonConvert.DeserializeObject<RootObject>(_content);
                }
            }
            else
            {
                ViewData["Message"] = "Invalid username and password";
            }
        }

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
                           where d.ApproveStatus == "APPROVE"
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
            if (HttpContext.Session.GetString("SUsername") != null)
            {
                SUsername = HttpContext.Session.GetString("SUsername");
                SPassword = HttpContext.Session.GetString("SPassword");
                await AmbilDataRole();
            }

            return Page();

        }
    }
}
