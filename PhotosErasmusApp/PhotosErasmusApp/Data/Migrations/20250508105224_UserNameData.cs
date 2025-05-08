using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotosErasmusApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserNameData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "MyUsers",
                type: "nvarchar(40)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "MyUsers");
        }
    }
}
