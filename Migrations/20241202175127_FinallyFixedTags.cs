using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendLaboratory.Migrations
{
    /// <inheritdoc />
    public partial class FinallyFixedTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7dd1e51a-606f-4dea-e6e3-08dbea521a91"),
                column: "CreateTime",
                value: new DateTime(2024, 12, 2, 17, 51, 27, 154, DateTimeKind.Utc).AddTicks(9953));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7dd1e51a-606f-4dea-e6e3-08dbea521a91"),
                column: "CreateTime",
                value: new DateTime(2024, 11, 26, 18, 33, 15, 918, DateTimeKind.Utc).AddTicks(7573));
        }
    }
}
