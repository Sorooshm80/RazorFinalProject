using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitIdToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VisitId",
                table: "Bookings",
                column: "VisitId",
                unique: true,
                filter: "[VisitId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Visits_VisitId",
                table: "Bookings",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id");
            migrationBuilder.Sql(@"
                UPDATE b
                SET b.VisitId = v.Id
                FROM Bookings b
                JOIN Visits v ON v.CustomerId = b.CustomerId
                             AND v.SessionId = b.SessionId
                             AND CAST(v.VisitDate AS date) = b.SessionDate
                WHERE b.Status = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Visits_VisitId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_VisitId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "VisitId",
                table: "Bookings");
        }
    }
}
