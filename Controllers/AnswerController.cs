using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly DataContext _ctx;

        public AnswerController(DataContext ctx)
        {
            _ctx = ctx;
        }
    }
}