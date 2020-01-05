using Microsoft.EntityFrameworkCore;

namespace LegalLib.Data
{
    public class LegalLibContext : DbContext
    {
        public LegalLibContext(DbContextOptions<LegalLibContext> options)
            : base(options)
        {
        }

        public DbSet<LegalLib.Models.tblCategory> tblCategory { get; set; }
        public DbSet<LegalLib.Models.tblCriteria> tblCriteria { get; set; }
        public DbSet<LegalLib.Models.tblKlasifikasi> tblKlasifikasi { get; set; }
        public DbSet<LegalLib.Models.tblLegalDocument> tblLegalDocument { get; set; }
        public DbSet<LegalLib.Models.tblDocKlasifikasi> tblDocKlasifikasi { get; set; }
        public DbSet<LegalLib.Models.tblDocKlasifikasi> tblRole { get; set; }
        public DbSet<LegalLib.Models.tblFileAttach> tblFileAttach { get; set; }
        public DbSet<LegalLib.Models.tblComments> tblComments { get; set; }


    }
}
