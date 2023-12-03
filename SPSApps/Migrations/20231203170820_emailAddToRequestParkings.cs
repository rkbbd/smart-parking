using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApps.Migrations
{
    /// <inheritdoc />
    public partial class emailAddToRequestParkings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestUserEmail",
                table: "RequestParkings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestUserEmail",
                table: "RequestParkings");
        }
    }
}
