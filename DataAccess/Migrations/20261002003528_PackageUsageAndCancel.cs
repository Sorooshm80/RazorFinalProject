using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PackageUsageAndCancel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsedSessions",
                table: "SessionPackages",
                newName: "FreeTimeUsed");

            migrationBuilder.RenameColumn(
                name: "TotalSessions",
                table: "SessionPackages",
                newName: "FreeTimeTotal");

            migrationBuilder.AddColumn<int>(
                name: "FixedTotal",
                table: "SessionPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FixedUsed",
                table: "SessionPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SessionPackageId",
                table: "Bookings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedTotal",
                table: "SessionPackages");

            migrationBuilder.DropColumn(
                name: "FixedUsed",
                table: "SessionPackages");

            migrationBuilder.DropColumn(
                name: "SessionPackageId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "FreeTimeUsed",
                table: "SessionPackages",
                newName: "UsedSessions");

            migrationBuilder.RenameColumn(
                name: "FreeTimeTotal",
                table: "SessionPackages",
                newName: "TotalSessions");
        }
    }
}
