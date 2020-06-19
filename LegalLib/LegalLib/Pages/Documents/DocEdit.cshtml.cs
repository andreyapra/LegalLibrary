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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LegalLib
{
    public class DocEditModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }
        public DocEditModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
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

        public List<TblCategory> TblCategory { get; set; }

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
        public IList<TblFileAttach> TblListFileAttach { get; set; }
        [BindProperty]
        public IList<TblDK> TblListDK { get; set; }
        [BindProperty]
        public TblDK TblRevisiDK { get; set; }
        [BindProperty]
        public IList<TblDK> TblListRevisiDK { get; set; }
        [BindProperty]
        public TblFileAttach TblRevisiFA { get; set; }
        [BindProperty]
        public IList<TblFileAttach> TblListRevisiFA { get; set; }

        [BindProperty]
        public TblLegalDocument TblLegalDocument { get; set; }
        [BindProperty]
        public TblLegalDocument TblOldDocument { get; set; }
        public SelectList CategorySL { get; set; }
        public SelectList CriteriaSL { get; set; }
        public SelectList KlasifikasiSL { get; set; }
        public SelectList RevDocumentSL { get; set; }

        public string SUsername { get; set; }
        public string SPassword { get; set; }
        public int SRole { get; set; }

        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }
        public int DocID { get; set; }
        public int DocumentID { get; set; }


        public int CheckRoleName(string rolename)
        {
            return _login.DataUsman.DataRole.Where(m => m.RoleName == rolename).Count();
        }
        public async Task AmbilDataRole()
        {
            //Panggil USMAN
            string Baseurl = "https://apps.pertamina.com/api/login/Users/Loginldap";
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

        public async Task LogActivity(string action)
        {
            string Username = HttpContext.Session.GetString("SUsername");
            //Logging Local
            TblLogActivity.UserID = Username;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "DOCUMENT";
            TblLogActivity.Action = action;
            TblLogActivity.Description = "DOCUMENTID=" + DocID;
            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject();
            oJsonObject.Add("username", Username);
            oJsonObject.Add("modul", "DOCUMENT");
            oJsonObject.Add("action", action + " DOCUMENTID=" + DocID);
            oJsonObject.Add("appname", "Online Library");

            var _Client = new HttpClient();
            var _response = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));
            var _content = await _response.Content.ReadAsStringAsync();

        }


        public string GetKlasifikasi(int id)
        {
            string Klasifikasi;
            Klasifikasi = _context.TblKlasifikasi.Where(m => m.KlasifikasiID == id).FirstOrDefault().Klasifikasi;

            return Klasifikasi;
        }

        public void PopulateRevDocument()
        {
            var DocQuery = from d in _context.TblLegalDocument
                           where d.ApproveStatus == "APPROVE"
                           where d.Status != "CABUT"
                           where d.TglAkhir > System.DateTime.Today
                           where d.IsActive == true
                           orderby d.NamaDocument
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
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            TblLegalDocument = await _context.TblLegalDocument.FirstOrDefaultAsync(m => m.DocumentID == id);
            TblListFileAttach = await _context.TblFileAttach.Where(m => m.DocumentID == id).Where(m => m.IsActive == true).ToListAsync();
            TblListDK = await _context.TblDK.Where(m => m.DocumentID == id).Where(m => m.IsActive == true).ToListAsync();
            TblOldDocument = TblLegalDocument;

            if (TblLegalDocument == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("SUsername") != null)
            {
                SUsername = HttpContext.Session.GetString("SUsername");
                SPassword = HttpContext.Session.GetString("SPassword");
                await AmbilDataRole();
            }

            SUsername = HttpContext.Session.GetString("SUsername");
            SRole = HttpContext.Session.GetInt32("SRole").GetValueOrDefault();

            if (SUsername == null)
            {
                return RedirectToPage("/Login");
            }
            else if (SRole < 2)
            {
                return RedirectToPage("/Denied");
            }

            PopulateCategory();
            PopulateCriteria();
            PopulateKlasifikasi();
            PopulateRevDocument();

            HttpContext.Session.SetInt32("SID", id.Value);
            HttpContext.Session.SetInt32("Revisi", TblLegalDocument.Revisi);

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
            TblLegalDocument.ApproveStatus = "PENDING";
            TblLegalDocument.IsActive = true;

            int oldid = HttpContext.Session.GetInt32("SID").Value;

            //cek nomor revisi
            int oldRev = HttpContext.Session.GetInt32("Revisi").Value;
            int newRev = TblLegalDocument.Revisi;

            if ((TblLegalDocument.Status=="REVISI") && (newRev > oldRev))
            {
                    //Revisi dokumen
                    int n = _context.TblLegalDocument.Count();
                    DocumentID = n + 1;

                    TblLegalDocument.DocumentID = n + 1;

                    TblLegalDocument.UploaderID = HttpContext.Session.GetString("SUsername");
                    TblLegalDocument.UploaderName = HttpContext.Session.GetString("SNama");
                    TblLegalDocument.UploaderEmail = HttpContext.Session.GetString("SEmail");
                    TblLegalDocument.TglUpload = System.DateTime.Now;

                    _context.TblLegalDocument.Add(TblLegalDocument);
                    //await _context.SaveChangesAsync();


                //add klasifikasi DK
                TblListDK = await _context.TblDK.Where(m => m.DocumentID == oldid).Where(m => m.IsActive == true).ToListAsync();

                foreach (TblDK dk in TblListDK)
                {
                    TblRevisiDK = new TblDK();
                    TblRevisiDK.DocumentID = DocumentID;
                    TblRevisiDK.KlasifikasiID = dk.KlasifikasiID;

                    TblRevisiDK.IsActive = true;
                    TblRevisiDK.ModifiedBy = HttpContext.Session.GetString("SUsername");
                    TblRevisiDK.ModifiedDate = System.DateTime.Now;
                    TblRevisiDK.CreatedBy = HttpContext.Session.GetString("SUsername");
                    TblRevisiDK.CreatedDate = System.DateTime.Now;

                    TblListRevisiDK.Add(TblRevisiDK);
                }

                _context.TblDK.AddRange(TblListRevisiDK);

                //add fileattach
                TblListFileAttach = await _context.TblFileAttach.Where(m => m.DocumentID == oldid).Where(m => m.IsActive == true).ToListAsync();

                foreach (TblFileAttach fa in TblListFileAttach)
                {
                    TblRevisiFA = new TblFileAttach();
                    TblRevisiFA.DocumentID = DocumentID;
                    TblRevisiFA.Filename = fa.Filename;

                    TblRevisiFA.IsActive = true;
                    TblRevisiFA.ModifiedBy = HttpContext.Session.GetString("SUsername");
                    TblRevisiFA.ModifiedDate = System.DateTime.Now;
                    TblRevisiFA.CreatedBy = HttpContext.Session.GetString("SUsername");
                    TblRevisiFA.CreatedDate = System.DateTime.Now;

                    TblListRevisiFA.Add(TblRevisiFA);

                    //copy file
                    string rootdir = "C:/BACKUP/uploads/";
                    string sourcefolder = oldid.ToString();
                    string destfolder = DocumentID.ToString();
                    var sourcefile = Path.Combine(rootdir, sourcefolder, fa.Filename);
                    var destfile = Path.Combine(rootdir, destfolder, fa.Filename);
                    
                    string folderpath = rootdir + destfolder;

                    if (Directory.Exists(folderpath) == false)
                    {
                        Directory.CreateDirectory(folderpath);
                    }
                    System.IO.File.Copy(sourcefile,destfile);


                }

                _context.TblFileAttach.AddRange(TblListRevisiFA);





                await _context.SaveChangesAsync();
                HttpContext.Session.SetInt32("SID", DocumentID);


            }
            else
            {
                //Save dokumen
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
            }

            DocID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
            await LogActivity("EDIT");

            return RedirectToPage(new { id = DocID});
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

            if (TblAddDK.KlasifikasiID != 0)
            {
                _context.TblDK.Add(TblAddDK);
                await _context.SaveChangesAsync();

                DocID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
                await LogActivity("ADDKLASIFIKASI");

            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostFileUpload()
        {
            if (Upload == null)
            {
                return RedirectToPage();
            }

            string rootdir = "C:/BACKUP/uploads/";
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

            DocID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
            await LogActivity("UPLOADFILE");

            return RedirectToPage();

        }
        private bool tblLegalDocumentExists(int id)
        {
            return _context.TblLegalDocument.Any(e => e.DocumentID == id);
        }

        public async Task<IActionResult> OnGetDeleteDK(int? dkid)
        {
            if (dkid == null)
            {
                return NotFound();
            }
            TblAddDK = await _context.TblDK.FirstOrDefaultAsync(m => m.DKID == dkid);
            if (TblAddDK == null)
            {
                return NotFound();
            }

            TblAddDK.IsActive = false;
            TblAddDK.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblAddDK.ModifiedDate = System.DateTime.Now;
            _context.Attach(TblAddDK).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            DocID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
            await LogActivity("DELETEKLASIFIKASI");

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteFile(int? fileid)
        {
            if (fileid == null)
            {
                return NotFound();
            }
            TblFileAttach = await _context.TblFileAttach.FirstOrDefaultAsync(m => m.FileID == fileid);
            if (TblFileAttach == null)
            {
                return NotFound();
            }

            TblFileAttach.IsActive = false;
            TblFileAttach.ModifiedBy = HttpContext.Session.GetString("SUsername");
            TblFileAttach.ModifiedDate = System.DateTime.Now;
            _context.Attach(TblFileAttach).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            DocID = HttpContext.Session.GetInt32("SID").GetValueOrDefault();
            await LogActivity("DELETEFILE");

            return RedirectToPage();
        }


    }
}
