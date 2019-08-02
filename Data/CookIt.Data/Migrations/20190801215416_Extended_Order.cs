namespace CookIt.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Extended_Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommentIssuer",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameIssuer",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentIssuer",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FullNameIssuer",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Orders");
        }
    }
}
