using System;
using System.Collections.Generic;
using System.Linq;
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
    public class LoginIndexModel : PageModel
    {
        private readonly LegalLib.Data.LegalLibContext _context;

        public IConfiguration Configuration { get; }
        public LoginIndexModel(LegalLib.Data.LegalLibContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [BindProperty][Required(ErrorMessage = "Tidak boleh kosong")]
        public string InputUsername { get; set; }
        [BindProperty][Required(ErrorMessage = "Tidak boleh kosong")]
        public string InputPassword { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblLogActivity TblLogActivity { get; set; }

        public string SUsername { get; set; }
        public List<TblCategory> TblCategory { get; set; }

        public string Message { get; set; }

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
        
        //User Parameter
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string RoleGroupName { get; set; }



        public async Task LogActivity()
        {
            string Usr = HttpContext.Session.GetString("SUsername");
            //Logging Local
            TblLogActivity.UserID = Usr;
            TblLogActivity.LogTime = System.DateTime.Now;
            TblLogActivity.Modul = "LOGIN";
            TblLogActivity.Action = "LOGIN";
            TblLogActivity.Description = "USER=" + Usr;

            _context.TblLogActivity.Add(TblLogActivity);
            await _context.SaveChangesAsync();

            //Logging API
            string Baseurl = Configuration["Setting:InsertLogURL"];
            string sContentType = "application/json";
            JObject oJsonObject = new JObject
            {
                { "username", Usr },
                { "modul", "LOGIN" },
                { "action", "LOGIN " + "USER=" + Usr },
                { "appname", "Online Library" }
            };

            var _Client = new HttpClient();
            _ = await _Client.PostAsync(Baseurl, new StringContent(oJsonObject.ToString(), Encoding.UTF8, sContentType));

        }

        public async Task<IActionResult> OnGet()
        {
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();
            return Page();
        }

        public async Task PanggilUsman()
        {
            //Panggil USMAN
            string Baseurl = "https://apps.pertamina.com/api/login/Users/Loginldap";
            string sContentType = "application/json"; // or application/xml

            JObject oJsonObject = new JObject
            {
                { "username", InputUsername },
                { "password", InputPassword },
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

                    Email = _login.dataLDAP.Data.Email;
                    UserID = _login.dataLDAP.Data.UserName;
                    Username = _login.dataLDAP.Data.NamaLengkap;
                    RoleGroupName = _login.DataUsman.DataRoleGroup[0].RoleGroupName;

                    switch (RoleGroupName)
                    {
                        case "Legal_User":
                            Role = 2;
                            break;
                        case "Legal_Mgr":
                            Role = 3;
                            break;
                        default:
                            Role = 1;
                            break;
                    }
                    //translate dari RoleGroupName

                    HttpContext.Session.SetInt32("SRole", Role);
                    HttpContext.Session.SetString("SNama", Username);
                    HttpContext.Session.SetString("SEmail", Email);
                    HttpContext.Session.SetString("SUsername", UserID);
                    HttpContext.Session.SetString("SPassword", InputPassword);
                    Message = HttpContext.Session.GetString("SUsername");
                    await LogActivity();

                }
            }
            else
            {
                Message = "Invalid username and password";
            }
        }

        public async Task LoginHC()
        {
            //Hardcode User
            switch (InputUsername)
            {
                case "manager":
                    HttpContext.Session.SetInt32("SRole", 3);
                    HttpContext.Session.SetString("SNama", "Nama Manager");
                    HttpContext.Session.SetString("SEmail", "manager@email.com");
                    HttpContext.Session.SetString("SUsername", InputUsername);
                    HttpContext.Session.SetString("SPassword", InputPassword);
                    Message = HttpContext.Session.GetString("SUsername");
                    await LogActivity();
                    break;

                case "legal":
                    HttpContext.Session.SetInt32("SRole", 2);
                    HttpContext.Session.SetString("SNama", "Nama Legal");
                    HttpContext.Session.SetString("SEmail", "legal@email.com");
                    HttpContext.Session.SetString("SUsername", InputUsername);
                    HttpContext.Session.SetString("SPassword", InputPassword);
                    Message = HttpContext.Session.GetString("SUsername");
                    await LogActivity();
                    break;

                case "user":
                    HttpContext.Session.SetInt32("SRole", 1);
                    HttpContext.Session.SetString("SNama", "Nama User");
                    HttpContext.Session.SetString("SEmail", "user@email.com");
                    HttpContext.Session.SetString("SUsername", InputUsername);
                    HttpContext.Session.SetString("SPassword", InputPassword);
                    Message = HttpContext.Session.GetString("SUsername");
                    await LogActivity();
                    break;

                default:
                    Message = "Invalid username and password";
                    break;
            }

        }


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            TblCategory = await _context.TblCategory.Where(m => m.IsActive == true).ToListAsync();

            //await LoginHC();
            await PanggilUsman();
            if (HttpContext.Session.GetString("SUsername") != null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}