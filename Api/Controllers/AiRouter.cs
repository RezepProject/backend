using backend.Entities;
using LayerSystemController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedResources;
using SharedResources.Entities;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AiRouter(DataContext ctx) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> PostAiRequest(string text)
    {
        var req = new Request
        {
            Text = text
        };

        // TODO

        return "";
    }
}