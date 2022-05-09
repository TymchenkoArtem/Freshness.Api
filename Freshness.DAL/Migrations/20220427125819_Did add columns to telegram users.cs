using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freshness.DAL.Migrations
{
    public partial class Didaddcolumnstotelegramusers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "TelegramBotOrderUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "TelegramBotOrderUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "TelegramBotOrderUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "TelegramBotOrderUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "TelegramBotCallUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "TelegramBotCallUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "TelegramBotCallUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "TelegramBotCallUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "TelegramBotOrderUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "TelegramBotOrderUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "TelegramBotOrderUsers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "TelegramBotOrderUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "TelegramBotCallUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "TelegramBotCallUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "TelegramBotCallUsers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "TelegramBotCallUsers");
        }
    }
}
