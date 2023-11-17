using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly DataContext _ctx;

        public ConfigController(DataContext ctx)
        {
            _ctx = ctx;
        }
    }
}