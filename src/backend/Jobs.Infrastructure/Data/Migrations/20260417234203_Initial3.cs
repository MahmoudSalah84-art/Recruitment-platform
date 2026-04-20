using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jobs_HrId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "HrId",
                table: "Jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HrId",
                table: "Jobs",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_HrId",
                table: "Jobs",
                column: "HrId");
        }
    }
}
