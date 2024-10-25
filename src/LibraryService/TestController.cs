namespace LibraryAPI.LibraryService;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Route("error")]
    public IActionResult GetError()
    {
        throw new InvalidOperationException("This is a test exception!");
    }
}
