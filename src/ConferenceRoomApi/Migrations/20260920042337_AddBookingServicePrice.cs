using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceRoomApi.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingServicePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BookingServiceLinks",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookingServiceLinks");
        }
    }
}
