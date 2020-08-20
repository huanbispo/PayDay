using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PayCompute.Persistence.Migrations
{
    public partial class UpdatedEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateJoin",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoined",
                table: "Employees",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateJoin",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
