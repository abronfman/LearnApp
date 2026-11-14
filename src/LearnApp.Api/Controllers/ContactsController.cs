using LearnApp.Api.Dtos;
using LearnApp.Api.Services;
using LearnApp.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearnApp.Api.Controllers;

[ApiController]                 // automatic 400s for invalid input, plus binding-source rules (see 3.9)
[Route("api/contacts")]
public class ContactsController(IContactService contacts, IOptions<ContactSettings> options) : ControllerBase
{
    private readonly ContactSettings _settings = options.Value;

    // GET api/contacts?search=an&sortBy=email&sortDir=desc&page=1&pageSize=2  -> 200
    [HttpGet]
    public async Task<ActionResult<PagedResult<ContactSummaryDto>>> Search([FromQuery] ContactQuery query, CancellationToken ct)
    {
        var pageSize = Math.Clamp(query.PageSize ?? _settings.DefaultPageSize, 1, _settings.MaxPageSize);
        return Ok(await contacts.SearchAsync(query, pageSize, ct));
    }

    // GET api/contacts/2  -> 200, or 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactDetailDto>> GetById(int id, CancellationToken ct) =>
        await contacts.GetByIdAsync(id, ct) is { } contact ? Ok(contact) : NotFound();

    // POST api/contacts  -> 201 Created, with a Location header pointing at the new contact
    [HttpPost]
    public async Task<ActionResult<ContactDetailDto>> Create(ContactRequest request, CancellationToken ct)
    {
        var created = await contacts.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT api/contacts/2  -> 204 No Content, or 404
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ContactRequest request, CancellationToken ct) =>
        await contacts.UpdateAsync(id, request, ct) ? NoContent() : NotFound();

    // DELETE api/contacts/2  -> 204 No Content, or 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) =>
        await contacts.DeleteAsync(id, ct) ? NoContent() : NotFound();
}