using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freshness.DAL.Migrations
{
    public partial class AddedaccessTokenandaccessTokenExpireDatetoworkertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AccessTokenExpireDate",
                table: "Workers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Container",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "AccessTokenExpireDate",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Container",
                table: "Orders");
        }
    }
}
