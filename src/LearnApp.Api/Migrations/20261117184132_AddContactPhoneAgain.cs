using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApp.Api.Migrations;

/// <inheritdoc />
public partial class _20261117184132_AddContactPhoneAgain : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Phone",
            table: "Contacts",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "Contacts",
            keyColumn: "Id",
            keyValue: 1,
            column: "Phone",
            value: "");

        migrationBuilder.UpdateData(
            table: "Contacts",
            keyColumn: "Id",
            keyValue: 2,
            column: "Phone",
            value: "");

        migrationBuilder.UpdateData(
            table: "Contacts",
            keyColumn: "Id",
            keyValue: 3,
            column: "Phone",
            value: "");

        migrationBuilder.UpdateData(
            table: "Contacts",
            keyColumn: "Id",
            keyValue: 4,
            column: "Phone",
            value: "");

        migrationBuilder.UpdateData(
            table: "Contacts",
            keyColumn: "Id",
            keyValue: 5,
            column: "Phone",
            value: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Phone",
            table: "Contacts");
    }
}
