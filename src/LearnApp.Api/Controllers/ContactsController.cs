using LearnApp.Api.Dtos;
using LearnApp.Api.Models;
using LearnApp.Api.Services;
using LearnApp.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearnApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController(IContactService contacts, IOptions<ContactSettings> options) : ControllerBase
    {
        private readonly ContactSettings _settings = options.Value;

        // GET api/contacts?take=5
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ContactSummaryDto>>> GetAll([FromQuery] int? take, CancellationToken ct)
        {
            var pageSize = Math.Clamp(take ?? _settings.DefaultPageSize, 1, _settings.MaxPageSize);
            if (pageSize > _settings.MaxPageSize)
            {
                return BadRequest($"The maximum page size is {_settings.MaxPageSize}.");
            }
            var contactsList = await contacts.GetAllAsync(pageSize, ct);
            return Ok(contactsList);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ContactDetailDto>> GetById(int id, CancellationToken ct) => await contacts.GetByIdAsync(id, ct) is { } contact ? Ok(contact) : NotFound();
    }
}
