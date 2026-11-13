using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApp.Api.Migrations;

/// <inheritdoc />
public partial class _20261113121516_AddGetCOntactsByCityProc : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Up: what "database update" runs. CREATE OR ALTER makes it safe to run more than once.
        migrationBuilder.Sql("""
        CREATE OR ALTER PROCEDURE dbo.GetContactsByCity @City nvarchar(100)
        AS
        BEGIN
            SET NOCOUNT ON;
            SELECT DISTINCT c.Id, c.FirstName, c.LastName, c.Email
            FROM dbo.Contacts AS c
            JOIN dbo.Addresses AS a ON a.ContactId = c.Id
            WHERE a.City = @City;
        END
        """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Down: how to undo it if this migration is ever rolled back.
        migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetContactsByCity;");
    }
}
