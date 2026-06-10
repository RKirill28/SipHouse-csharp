using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SipHouseCSharpBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedToGuidForFileIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PdfUrls",
                table: "projects",
                newName: "PdfFileIds");

            migrationBuilder.AlterColumn<Guid>(
                name: "FileId",
                table: "images",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PdfFileIds",
                table: "projects",
                newName: "PdfUrls");

            migrationBuilder.AlterColumn<long>(
                name: "FileId",
                table: "images",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");
        }
    }
}
