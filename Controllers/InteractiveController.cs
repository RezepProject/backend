using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InteractiveController : ControllerBase
    {
        private readonly DataContext _ctx;

        public InteractiveController(DataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetTestData()
        {
            return new List<string> { "test", "test2" };
        }
    }
}