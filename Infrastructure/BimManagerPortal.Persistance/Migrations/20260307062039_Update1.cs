using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BimManagerPortal.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 ALTER TABLE "BigDataPlugins"
                                 ALTER COLUMN "JsonData" TYPE bytea
                                 USING convert_to("JsonData", 'UTF8');
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JsonData",
                table: "BigDataPlugins",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");
        }
    }
}
