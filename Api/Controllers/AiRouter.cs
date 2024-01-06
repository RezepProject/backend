using LayerSystemController;
using Microsoft.AspNetCore.Mvc;
using SharedResources.Entities;
using SharedResources.Repositories;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AiRouter(DataContext ctx) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Response>> PostAiRequest(string text)
    {
        var req = new Request
        {
            Text = text
        };

        RequestHandler.AddRequest(req);

        var res = ResponseHandler.GetById(req.Id);

        int cnt = 0;
        while (res == null)
        {
            await Task.Delay(100);
            res = ResponseHandler.GetById(req.Id);
            if (cnt++ > 100)
            {
                return BadRequest("Request timed out!");
            }
        }

        return res;
    }
}