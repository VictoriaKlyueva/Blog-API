using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendLaboratory.Migrations
{
    /// <inheritdoc />
    public partial class PostsUsersEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("0c64569f-5675-484b-e6eb-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("47aa0a33-2b86-4039-e6e9-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("553f5361-428a-4122-e6e6-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7dd0e51a-606f-4dea-e6e3-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("7dd1e51a-606f-4dea-e6e3-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("86acb301-05ff-4822-e6e7-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("d774dd11-2600-46ab-e6e4-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e463050a-d659-433d-e6ea-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e587312f-4df7-4879-e6e8-08dbea521a91"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("fb1f2ce1-6943-420f-e6ec-08dbea521a91"));

            migrationBuilder.CreateTable(
                name: "PostsUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailStatus = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsUsers", x => new { x.PostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PostsUsers_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostsUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostsUsers_UserId",
                table: "PostsUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsUsers");

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "CreateTime", "Name" },
                values: new object[,]
                {
                    { new Guid("0c64569f-5675-484b-e6eb-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6600), "косплей" },
                    { new Guid("341aa6d9-cf7b-4a99-e6e5-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6543), "18+" },
                    { new Guid("47aa0a33-2b86-4039-e6e9-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6583), "теория_заговора" },
                    { new Guid("553f5361-428a-4122-e6e6-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6553), "приколы" },
                    { new Guid("7dd0e51a-606f-4dea-e6e3-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6519), "история" },
                    { new Guid("7dd1e51a-606f-4dea-e6e3-08dbea521a91"), new DateTime(2024, 12, 2, 17, 51, 27, 154, DateTimeKind.Utc).AddTicks(9953), "задор" },
                    { new Guid("86acb301-05ff-4822-e6e7-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6563), "it" },
                    { new Guid("d774dd11-2600-46ab-e6e4-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6539), "еда" },
                    { new Guid("e463050a-d659-433d-e6ea-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6592), "соцсети" },
                    { new Guid("e587312f-4df7-4879-e6e8-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6573), "интернет" },
                    { new Guid("fb1f2ce1-6943-420f-e6ec-08dbea521a91"), new DateTime(2023, 11, 21, 12, 24, 15, 810, DateTimeKind.Utc).AddTicks(6612), "преступление" }
                });
        }
    }
}
