using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LegalLib.Migrations
{
    public partial class LegalLib : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCategory", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "TblCriteria",
                columns: table => new
                {
                    CriteriaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Criteria = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCriteria", x => x.CriteriaID);
                });

            migrationBuilder.CreateTable(
                name: "TblKlasifikasi",
                columns: table => new
                {
                    KlasifikasiID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Klasifikasi = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblKlasifikasi", x => x.KlasifikasiID);
                });

            migrationBuilder.CreateTable(
                name: "TblLegalDocument",
                columns: table => new
                {
                    DocumentID = table.Column<int>(nullable: false),
                    Nomor = table.Column<string>(nullable: true),
                    NamaDocument = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Perihal = table.Column<string>(nullable: true),
                    Regulation = table.Column<string>(nullable: true),
                    Chapter = table.Column<string>(nullable: true),
                    Requirement = table.Column<string>(nullable: true),
                    LegalRisk = table.Column<string>(nullable: true),
                    BusinessRisk = table.Column<string>(nullable: true),
                    PeopleRisk = table.Column<string>(nullable: true),
                    EnvironmentRisk = table.Column<string>(nullable: true),
                    Methods = table.Column<string>(nullable: true),
                    Authority = table.Column<string>(nullable: true),
                    TglMulai = table.Column<DateTime>(nullable: false),
                    TglAkhir = table.Column<DateTime>(nullable: false),
                    Revisi = table.Column<int>(nullable: false),
                    RevDocument = table.Column<int>(nullable: false),
                    Permit = table.Column<string>(nullable: true),
                    PermitDueDate = table.Column<DateTime>(nullable: false),
                    ReportDueDate = table.Column<DateTime>(nullable: false),
                    Catatan = table.Column<string>(nullable: true),
                    ApproveStatus = table.Column<string>(nullable: true),
                    UploaderID = table.Column<string>(nullable: true),
                    UploaderName = table.Column<string>(nullable: true),
                    UploaderEmail = table.Column<string>(nullable: true),
                    TglUpload = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    CriteriaID = table.Column<int>(nullable: false),
                    TblCategoryCategoryID = table.Column<int>(nullable: true),
                    TblCriteriaCriteriaID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblLegalDocument", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_TblLegalDocument_TblCategory_TblCategoryCategoryID",
                        column: x => x.TblCategoryCategoryID,
                        principalTable: "TblCategory",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblLegalDocument_TblCriteria_TblCriteriaCriteriaID",
                        column: x => x.TblCriteriaCriteriaID,
                        principalTable: "TblCriteria",
                        principalColumn: "CriteriaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblComment",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    TblLegalDocumentDocumentID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblComment", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_TblComment_TblLegalDocument_TblLegalDocumentDocumentID",
                        column: x => x.TblLegalDocumentDocumentID,
                        principalTable: "TblLegalDocument",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblDK",
                columns: table => new
                {
                    DKID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    DocumentID = table.Column<int>(nullable: false),
                    KlasifikasiID = table.Column<int>(nullable: false),
                    TblLegalDocumentDocumentID = table.Column<int>(nullable: true),
                    TblKlasifikasiKlasifikasiID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDK", x => x.DKID);
                    table.ForeignKey(
                        name: "FK_TblDK_TblKlasifikasi_TblKlasifikasiKlasifikasiID",
                        column: x => x.TblKlasifikasiKlasifikasiID,
                        principalTable: "TblKlasifikasi",
                        principalColumn: "KlasifikasiID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TblDK_TblLegalDocument_TblLegalDocumentDocumentID",
                        column: x => x.TblLegalDocumentDocumentID,
                        principalTable: "TblLegalDocument",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TblFileAttach",
                columns: table => new
                {
                    FileID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    DocumentID = table.Column<int>(nullable: false),
                    TblLegalDocumentDocumentID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblFileAttach", x => x.FileID);
                    table.ForeignKey(
                        name: "FK_TblFileAttach_TblLegalDocument_TblLegalDocumentDocumentID",
                        column: x => x.TblLegalDocumentDocumentID,
                        principalTable: "TblLegalDocument",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblComment_TblLegalDocumentDocumentID",
                table: "TblComment",
                column: "TblLegalDocumentDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_TblDK_TblKlasifikasiKlasifikasiID",
                table: "TblDK",
                column: "TblKlasifikasiKlasifikasiID");

            migrationBuilder.CreateIndex(
                name: "IX_TblDK_TblLegalDocumentDocumentID",
                table: "TblDK",
                column: "TblLegalDocumentDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_TblFileAttach_TblLegalDocumentDocumentID",
                table: "TblFileAttach",
                column: "TblLegalDocumentDocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_TblLegalDocument_TblCategoryCategoryID",
                table: "TblLegalDocument",
                column: "TblCategoryCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_TblLegalDocument_TblCriteriaCriteriaID",
                table: "TblLegalDocument",
                column: "TblCriteriaCriteriaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblComment");

            migrationBuilder.DropTable(
                name: "TblDK");

            migrationBuilder.DropTable(
                name: "TblFileAttach");

            migrationBuilder.DropTable(
                name: "TblKlasifikasi");

            migrationBuilder.DropTable(
                name: "TblLegalDocument");

            migrationBuilder.DropTable(
                name: "TblCategory");

            migrationBuilder.DropTable(
                name: "TblCriteria");
        }
    }
}
