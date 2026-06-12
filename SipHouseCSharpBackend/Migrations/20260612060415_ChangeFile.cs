using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SipHouseCSharpBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PdfFileIds",
                table: "projects",
                newName: "PdfFilePaths");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "images",
                newName: "FilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PdfFilePaths",
                table: "projects",
                newName: "PdfFileIds");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "images",
                newName: "FileId");
        }
    }
}
