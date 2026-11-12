using LearnApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnApp.Api.Controllers
{
    [ApiController]
    [Route("api/di-demo")]
    public class DiDemoController(ITransientProbe transient, IScopedProbe scoped, ISingletonProbe singleton, ProbeReporter reporter) : ControllerBase
    {
        public IActionResult Get() => Ok(new
        {
            controller = new { transient = transient.Id, scoped = scoped.Id, singleton = singleton.Id },
            reporter = reporter.Snapshot()
        });
    }
}
