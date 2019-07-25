namespace CookIt.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Added_ColumnType_To_RecipeInstructions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecipeInstructions",
                table: "Recipes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RecipeInstructions",
                table: "Recipes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
