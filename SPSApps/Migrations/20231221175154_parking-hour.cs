using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApps.Migrations
{
    /// <inheritdoc />
    public partial class parkinghour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Hour",
                table: "RequestParkings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "RequestParkings");
        }
    }
}
