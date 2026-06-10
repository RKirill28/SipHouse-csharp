using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SipHouseCSharpBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUrlToFileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "images");

            migrationBuilder.AddColumn<long>(
                name: "FileId",
                table: "images",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "images");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "images",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
