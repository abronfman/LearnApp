using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnApp.Api.Migrations;

/// <inheritdoc />
public partial class _20261112203123_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Contacts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contacts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Addresses",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                ContactId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Addresses_Contacts_ContactId",
                    column: x => x.ContactId,
                    principalTable: "Contacts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Contacts",
            columns: new[] { "Id", "Email", "FirstName", "LastName" },
            values: new object[,]
            {
                { 1, "maria.lopez@example.com", "Maria", "Lopez" },
                { 2, "james.carter@example.com", "James", "Carter" },
                { 3, "aisha.khan@example.com", "Aisha", "Khan" },
                { 4, "daniel.kim@example.com", "Daniel", "Kim" },
                { 5, "sofia.rossi@example.com", "Sofia", "Rossi" }
            });

        migrationBuilder.InsertData(
            table: "Addresses",
            columns: new[] { "Id", "City", "ContactId", "PostalCode", "State", "Street" },
            values: new object[,]
            {
                { 1, "Springfield", 1, "62701", "IL", "12 Oak St" },
                { 2, "Madison", 1, "53703", "WI", "400 Lake Ave" },
                { 3, "Fairview", 2, "37062", "TN", "9 Pine Rd" },
                { 4, "Springfield", 3, "62704", "IL", "77 Maple Dr" },
                { 5, "Riverside", 4, "92501", "CA", "5 River Way" },
                { 6, "Greenville", 4, "29601", "SC", "88 Cedar Ln" }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Addresses_ContactId",
            table: "Addresses",
            column: "ContactId");

        migrationBuilder.CreateIndex(
            name: "IX_Contacts_Email",
            table: "Contacts",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Addresses");

        migrationBuilder.DropTable(
            name: "Contacts");
    }
}
