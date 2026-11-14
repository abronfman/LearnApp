using Microsoft.AspNetCore.Mvc;

namespace LearnApp.Api.Controllers;

[ApiController]
[Route("api/error-demo")]
public class ErrorDemoController : ControllerBase
{
    // Simulates a bug, so you can see what the client receives.
    [HttpGet]
    public IActionResult Boom() => throw new InvalidOperationException("Database password is hunter2");
}