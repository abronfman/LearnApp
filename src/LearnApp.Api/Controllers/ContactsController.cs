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
        public ActionResult<IReadOnlyList<Contact>> GetAll([FromQuery] int? take)
        {
            var pageSize =Math.Clamp(take ?? _settings.DefaultPageSize, 1, _settings.MaxPageSize);
            if (pageSize > _settings.MaxPageSize)
            {
                return BadRequest($"The maximum page size is {_settings.MaxPageSize}.");
            }
            var contactsList = contacts.GetAll(pageSize);
            return Ok(contactsList);
        }
        [HttpGet("{id:int}")]
        public ActionResult<Contact> GetById(int id) => contacts.GetById(id) is { } contact ? Ok(contact) : NotFound();
    }
}
