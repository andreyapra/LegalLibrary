using System;
using System.ComponentModel.DataAnnotations;

namespace LegalLib.Models
{
    public class tblCriteria
    {
        [Key]
        public int CriteriaID { get; set; }
        public string Criteria { get; set; }
        public string Description { get; set; }

    }

    public class tblCategory
    {
        [Key]
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

    }

    public class tblKlasifikasi
    {
        [Key]
        public int KlasifikasiID { get; set; }
        public string Klasifikasi { get; set; }

    }

    public class tblLegalDocument
    {
        public tblLegalDocument()
        {
            ApproveStatus = 0;
            TglUpload = System.DateTime.Today;
            Revisi = 0;
            RevDocument = 0;
        }

        [Key]
        public int DocumentID { get; set; }
        public string Nomor { get; set; }
        public string NamaDocument { get; set; }
        public int Status { get; set; }
        public string Perihal { get; set; }
        public string Regulation { get; set; }
        public string Chapter { get; set; }
        public string Requirement { get; set; }
        public string LegalRisk { get; set; }
        public string BusinessRisk { get; set; }
        public string PeopleRisk { get; set; }
        public string EnvironmentRisk { get; set; }
        public string Methods { get; set; }
        public string Authority { get; set; }
        [DataType(DataType.Date)]
        public DateTime TglMulai { get; set; }
        [DataType(DataType.Date)]
        public DateTime TglAkhir { get; set; }

        public int Revisi { get; set; }

        public int RevDocument { get; set; }
        public string Permit { get; set; }
        [DataType(DataType.Date)]
        public DateTime PermitDueDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReportDueDate { get; set; }
        public string Catatan { get; set; }
        public int ApproveStatus { get; set; }
        public string UploaderID { get; set; }
        public string UploaderName { get; set; }
        public string UploaderEmail { get; set; }
        [DataType(DataType.Date)]
        public DateTime TglUpload { get; set; }

        public int CriteriaID { get; set; }
        public int CategoryID { get; set; }


    }
    public class tblDocKlasifikasi
    {
        [Key]
        public int DKID { get; set; }
        public int DocumentID { get; set; }
        public int KlasifkasiID { get; set; }

    }

    public class tblRole
    {
        [Key]
        public int RoleID { get; }
        public string RoleName { get; }

    }
}
