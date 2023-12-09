using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderApplicaion.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Orders",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Orders", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "OutboxEvents",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        EventData = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        PublishedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
                //constraints: table =>
                //{
                //    table.PrimaryKey("PK_OutboxEvents", x => x.Id);
                //});

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "TotalAmount" },
                values: new object[,]
                {
                    { 1, "Cust1", 100.50m },
                    { 2, "Cust2", 200.00m }
                });

            migrationBuilder.InsertData(
                table: "OutboxEvents",
                columns: new[] { "Id", "CreatedOn", "EventData", "EventId", "EventType", "PublishedOn" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 12, 9, 17, 45, 45, 643, DateTimeKind.Utc).AddTicks(6343), "EventData1", new Guid("599fff65-d76c-4063-b911-2689cea5ec7b"), "Event1", null },
                    { 2, new DateTime(2023, 12, 9, 17, 45, 45, 643, DateTimeKind.Utc).AddTicks(6346), "EventData2", new Guid("1d6adf1a-57e3-41d0-8011-03e9b1bce8f6"), "Event2", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OutboxEvents");
        }
    }
}
