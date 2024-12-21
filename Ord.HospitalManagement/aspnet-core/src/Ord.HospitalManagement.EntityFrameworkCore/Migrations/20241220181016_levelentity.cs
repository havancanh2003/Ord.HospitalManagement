using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ord.HospitalManagement.Migrations
{
    /// <inheritdoc />
    public partial class levelentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Ward");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Ward",
                newName: "LevelWard");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "District",
                newName: "LevelDistrict");

            migrationBuilder.AddColumn<string>(
                name: "DistrictCode",
                table: "Ward",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceCode",
                table: "Ward",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "LevelProvince",
                table: "Province",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceCode",
                table: "District",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictCode",
                table: "Ward");

            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "Ward");

            migrationBuilder.DropColumn(
                name: "LevelProvince",
                table: "Province");

            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "District");

            migrationBuilder.RenameColumn(
                name: "LevelWard",
                table: "Ward",
                newName: "ProvinceId");

            migrationBuilder.RenameColumn(
                name: "LevelDistrict",
                table: "District",
                newName: "ProvinceId");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Ward",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
