using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiNetCore.Data.EFContext.Migrations
{
    /// <inheritdoc />
    public partial class AddingGenericColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Musician",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangeAt",
                table: "Musician",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "Revoked",
                table: "Musician",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Band",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangeAt",
                table: "Band",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Revoked",
                table: "Band",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Musician");

            migrationBuilder.DropColumn(
                name: "LastChangeAt",
                table: "Musician");

            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "Musician");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Band");

            migrationBuilder.DropColumn(
                name: "LastChangeAt",
                table: "Band");

            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "Band");
        }
    }
}
