using Microsoft.EntityFrameworkCore;

namespace LegalLib.Data
{
    public class LegalLibContext : DbContext
    {
        public LegalLibContext(DbContextOptions<LegalLibContext> options)
            : base(options)
        {
        }

        public DbSet<LegalLib.Models.TblCategory> TblCategory { get; set; }
        public DbSet<LegalLib.Models.TblCriteria> TblCriteria { get; set; }
        public DbSet<LegalLib.Models.TblKlasifikasi> TblKlasifikasi { get; set; }
        public DbSet<LegalLib.Models.TblLegalDocument> TblLegalDocument { get; set; }
        public DbSet<LegalLib.Models.TblDK> TblDK { get; set; }
        public DbSet<LegalLib.Models.TblFileAttach> TblFileAttach { get; set; }
        public DbSet<LegalLib.Models.TblComment> TblComment { get; set; }
        public DbSet<LegalLib.Models.TblLogActivity> TblLogActivity { get; set; }


    }
}
