namespace LearnApp.Api.Dtos
{
    public record ContactSummaryDto(int Id, string FullName, string Email, int AddressCount);
    public record AddressDto(int Id, string Street, string City, string State, string PostalCode);
    public record ContactDetailDto(int Id, string FirstName, string LastName, string Email, IReadOnlyList<AddressDto> Addresses);

    // Shape of one row returned by dbo.GetContactsByCity.
    // EF Core matches columns to properties BY NAME, so these names must match the procedure's columns.
    public class ContactByCityRow
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
    }
}

