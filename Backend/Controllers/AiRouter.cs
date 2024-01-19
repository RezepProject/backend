using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AiRouter(DataContext ctx) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> PostAiRequest(string text)
    {
        return "";
    }
}