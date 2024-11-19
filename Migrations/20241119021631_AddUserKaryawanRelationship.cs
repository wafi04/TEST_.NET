using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrudApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserKaryawanRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Karyawan",
                columns: table => new
                {
                    Nik = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TanggalLahir = table.Column<DateTime>(type: "date", nullable: false),
                    Alamat = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Negara = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JenisKelamin = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karyawan", x => x.Nik);
                    table.ForeignKey(
                        name: "FK_Karyawan_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Karyawan_UserId",
                table: "Karyawan",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Karyawan");
        }
    }
}
