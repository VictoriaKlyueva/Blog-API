using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLaboratory.Migrations
{
    /// <inheritdoc />
    public partial class BlacklistTableRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlackTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Blacktoken = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackTokens", x => x.Id);
                });
        }
    }
}
