﻿using System;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

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

        [BindProperty]
        public List<TblComment> TblComment { get;set; }
        [BindProperty]
        public TblComment TblNewComment { get; set; }
        public List<TblCategory> TblCategory { get; set; }

        public string SUsername { get; set; }
        public string SPassword { get; set; }
        public int SRole { get; set; }


        public int DocumentID { get; set; }

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
