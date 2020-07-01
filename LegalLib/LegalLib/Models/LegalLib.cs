using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegalLib.Models
{
    public class TblCriteria
    {
        [Key]
        public int CriteriaID { get; set; }
        public string Criteria { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
        public ICollection<TblLegalDocument> TblLegalDocuments { get; set; }
    }

    public class TblCategory
    {
        [Key]
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
        public ICollection<TblLegalDocument> TblLegalDocuments { get; set; }
    }

    public class TblKlasifikasi
    {
        [Key]
        public int KlasifikasiID { get; set; }
        public string Klasifikasi { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
        public ICollection<TblDK> TblDKs { get; set; }
    }

    public class TblLegalDocument
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentID { get; set; }
        [Display(Name = "Nomor Document")]
        public string Nomor { get; set; }
        [Display(Name ="Nama Document")]
        public string NamaDocument { get; set; }
        public string Status { get; set; }
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
        [Display(Name ="Tanggal Berlaku")]
        public DateTime TglMulai { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Berakhir")]
        public DateTime TglAkhir { get; set; }
        public int Revisi { get; set; }
        public int RevDocument { get; set; }
        public string Permit { get; set; }
        [DataType(DataType.Date)]
        public DateTime PermitDueDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReportDueDate { get; set; }
        public string Catatan { get; set; }
        [DataType(DataType.Date)]
        public DateTime? TglApprove { get; set; }
        public string ApproveStatus { get; set; }
        public string UploaderID { get; set; }
        public string UploaderName { get; set; }
        public string UploaderEmail { get; set; }
        [DataType(DataType.Date)]
        public DateTime TglUpload { get; set; }
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }
        [Display(Name = "Criteria")]
        public int CriteriaID { get; set; }
        public TblCategory TblCategory { get; set; }
        public TblCriteria TblCriteria { get; set; }
        public ICollection<TblFileAttach> TblFileAttaches { get; set; }
        public ICollection<TblDK> TblDKs { get; set; }
        public ICollection<TblComment> TblComments { get; set; }
        public bool IsActive { get; set; }

    }
    public class TblDK
    {
        [Key]
        public int DKID { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
        public int DocumentID { get; set; }
        public int KlasifikasiID { get; set; }
        public TblLegalDocument TblLegalDocument { get; set; }
        public TblKlasifikasi TblKlasifikasi { get; set; }
        public bool IsActive { get; set; }
    }

    public class TblFileAttach
    {
        [Key]
        public int FileID { get; set; }
        public string Filename { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
        public int DocumentID { get; set; }
        public TblLegalDocument TblLegalDocument { get; set; }
        public bool IsActive { get; set; }

    }

    public class TblComment
    {
        [Key]
        public int CommentID { get; set; }
        public int DocumentID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public TblLegalDocument TblLegalDocument { get; set; }
        public bool IsActive { get; set; }

    }
    public class TblLogActivity
    {
        [Key]
        public int LogID { get; set; }
        public string UserID { get; set; }
        [DataType(DataType.Date)]
        public DateTime LogTime { get; set; }
        public string Modul { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }

    }

}
