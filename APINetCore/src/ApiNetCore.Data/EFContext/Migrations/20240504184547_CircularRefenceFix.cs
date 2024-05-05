using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiNetCore.Data.EFContext.Migrations
{
    /// <inheritdoc />
    public partial class CircularRefenceFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BandMusician_Band_BandsId",
                table: "BandMusician");

            migrationBuilder.DropForeignKey(
                name: "FK_BandMusician_Musician_MusiciansId",
                table: "BandMusician");

            migrationBuilder.RenameColumn(
                name: "MusiciansId",
                table: "BandMusician",
                newName: "MusicianId");

            migrationBuilder.RenameColumn(
                name: "BandsId",
                table: "BandMusician",
                newName: "BandId");

            migrationBuilder.RenameIndex(
                name: "IX_BandMusician_MusiciansId",
                table: "BandMusician",
                newName: "IX_BandMusician_MusicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_BandMusician_Band_BandId",
                table: "BandMusician",
                column: "BandId",
                principalTable: "Band",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BandMusician_Musician_MusicianId",
                table: "BandMusician",
                column: "MusicianId",
                principalTable: "Musician",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BandMusician_Band_BandId",
                table: "BandMusician");

            migrationBuilder.DropForeignKey(
                name: "FK_BandMusician_Musician_MusicianId",
                table: "BandMusician");

            migrationBuilder.RenameColumn(
                name: "MusicianId",
                table: "BandMusician",
                newName: "MusiciansId");

            migrationBuilder.RenameColumn(
                name: "BandId",
                table: "BandMusician",
                newName: "BandsId");

            migrationBuilder.RenameIndex(
                name: "IX_BandMusician_MusicianId",
                table: "BandMusician",
                newName: "IX_BandMusician_MusiciansId");

            migrationBuilder.AddForeignKey(
                name: "FK_BandMusician_Band_BandsId",
                table: "BandMusician",
                column: "BandsId",
                principalTable: "Band",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BandMusician_Musician_MusiciansId",
                table: "BandMusician",
                column: "MusiciansId",
                principalTable: "Musician",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
