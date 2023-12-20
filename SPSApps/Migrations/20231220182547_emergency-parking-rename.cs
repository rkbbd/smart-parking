using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSApps.Migrations
{
    /// <inheritdoc />
    public partial class emergencyparkingrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmargencyFairPerParking",
                table: "Buildings",
                newName: "EmergencyFairPerParking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmergencyFairPerParking",
                table: "Buildings",
                newName: "EmargencyFairPerParking");
        }
    }
}
