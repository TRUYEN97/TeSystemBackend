using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeSystemBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFullNameFromTeams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Teams"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Teams",
                type: "varchar(400)",
                nullable: false
                ).Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
