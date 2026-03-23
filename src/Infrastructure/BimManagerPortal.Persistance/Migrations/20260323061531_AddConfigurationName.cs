using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BimManagerPortal.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfigurationName",
                table: "BigDataPlugins",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfigurationName",
                table: "BigDataPlugins");
        }
    }
}
