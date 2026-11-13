using LearnApp.Api.Data;
using LearnApp.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnApp.Api.Controllers;

[ApiController]
[Route("api/ef-demo")]
public class EfDemoController(AppDbContext db) : ControllerBase
{
    [HttpGet("tracking")]
    public async Task<IActionResult> Tracking()
    {
        var tracked = await db.Contacts.FirstAsync(c => c.Id == 1);                  // tracked by default
        var untracked = await db.Contacts.AsNoTracking().FirstAsync(c => c.Id == 2);   // not tracked

        tracked.FirstName += "!";   // change it in memory (no SaveChanges, so nothing is saved)

        return Ok(new
        {
            trackedState = db.Entry(tracked).State.ToString(),     // "Modified": EF noticed the change
            untrackedState = db.Entry(untracked).State.ToString(),   // "Detached": EF isn't watching it
            entriesBeingTracked = db.ChangeTracker.Entries().Count()
        });
    }

    [HttpGet("n-plus-one")]
    public async Task<IActionResult> NPlusOne() 
    {
        var contacts = await db.Contacts.AsNoTracking().ToListAsync();
        var result = new List<object>();

        foreach (var c in contacts)        
        {
            var count = await db.Addresses.CountAsync(a => a.ContactId == c.Id);   // one query per contact
            result.Add(new { c.Id, AddressCount = count });
        }
        return Ok(result);
    }

    [HttpGet("by-city/{city}")]
    public async Task<IActionResult> ByCity(string city)
    {
        // The {city} inside this interpolated string becomes a SQL PARAMETER (@p0), not pasted text.
        var rows = await db.Database
            .SqlQuery<ContactByCityRow>($"EXEC dbo.GetContactsByCity @City = {city}")
            .ToListAsync();
        return Ok(rows);

        // NEVER do this. Concatenation pastes user input into the SQL: classic SQL injection.
        // db.Database.SqlQueryRaw<ContactByCityRow>("EXEC dbo.GetContactsByCity @City = '" + city + "'")
    }
}