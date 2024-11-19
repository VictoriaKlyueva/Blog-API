using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendLaboratory.Migrations
{
    /// <inheritdoc />
    public partial class CommunityData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Communities",
                columns: new[] { "Id", "CreateTime", "Description", "IsClosed", "Name", "SubscribersCount" },
                values: new object[,]
                {
                    { new Guid("21db62c6-a964-45c1-17e0-08dbea521a96"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6622), "Место, помещение, где собираются масоны для проведения своих собраний, чаще называемых работами", true, "Масонская ложа", 44 },
                    { new Guid("b9851a35-b836-47f8-17e2-08dbea521a96"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6705), "Информационные технологии связаны с изучением методов и средств сбора, обработки и передачи данных с целью получения информации нового качества о состоянии объекта, процесса или явления", false, "IT <3", 31 },
                    { new Guid("c5639aab-3a25-4efc-17e1-08dbea521a96"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6695), "Без длинных предисловий: мужчина умер", false, "Следствие вели с Л. Каневским", 34 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Communities",
                keyColumn: "Id",
                keyValue: new Guid("21db62c6-a964-45c1-17e0-08dbea521a96"));

            migrationBuilder.DeleteData(
                table: "Communities",
                keyColumn: "Id",
                keyValue: new Guid("b9851a35-b836-47f8-17e2-08dbea521a96"));

            migrationBuilder.DeleteData(
                table: "Communities",
                keyColumn: "Id",
                keyValue: new Guid("c5639aab-3a25-4efc-17e1-08dbea521a96"));
        }
    }
}
