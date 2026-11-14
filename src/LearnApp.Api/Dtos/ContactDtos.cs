using System.ComponentModel.DataAnnotations;

namespace LearnApp.Api.Dtos;

public record ContactSummaryDto(int Id, string FullName, string Email, int AddressCount);

public record AddressDto(int Id, string Street, string City, string State, string PostalCode);

public record ContactDetailDto(int Id, string FirstName, string LastName, string Email, string? Phone, IReadOnlyList<AddressDto> Addresses);

public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount) 
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

public class ContactByCityRow 
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
}

// ---------- Requests -----------------
public class ContactRequest 
{
    [Required, StringLength(100)] public string FirstName { get; set; } = "";
    [Required, StringLength(100)] public string LastName { get; set; } = "";
    [Required, EmailAddress, StringLength(256)] public string Email { get; set; } = "";
    [Phone, StringLength(20)] public string? Phone { get; set; }
}

public class ContactQuery 
{
    public string? Search { get; set; }
    public string? SortBy { get; set; }      // "lastName" (default), "firstName", or "email"
    public string? SortDir { get; set; }     // "asc" (default) or "desc"
    [Range(1, 10_000)] public int Page { get; set; } = 1;
    public int? PageSize { get; set; }       // capped by ContactSettings.MaxPageSize
}
