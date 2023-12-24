using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DispatcherService.Migrations
{
    public partial class body : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "body",
                table: "OutboxEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OutboxEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "EventId", "body" },
                values: new object[] { new DateTime(2023, 12, 10, 17, 6, 5, 263, DateTimeKind.Utc).AddTicks(462), new Guid("fcc465d2-76a2-41e4-b3fa-df0544b8fab6"), "message1" });

            migrationBuilder.UpdateData(
                table: "OutboxEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "EventId", "body" },
                values: new object[] { new DateTime(2023, 12, 10, 17, 6, 5, 263, DateTimeKind.Utc).AddTicks(467), new Guid("cf190122-a772-4015-985b-43f6c4849438"), "message2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "body",
                table: "OutboxEvents");

            migrationBuilder.UpdateData(
                table: "OutboxEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "EventId" },
                values: new object[] { new DateTime(2023, 12, 9, 17, 45, 45, 643, DateTimeKind.Utc).AddTicks(6343), new Guid("599fff65-d76c-4063-b911-2689cea5ec7b") });

            migrationBuilder.UpdateData(
                table: "OutboxEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedOn", "EventId" },
                values: new object[] { new DateTime(2023, 12, 9, 17, 45, 45, 643, DateTimeKind.Utc).AddTicks(6346), new Guid("1d6adf1a-57e3-41d0-8011-03e9b1bce8f6") });
        }
    }
}
