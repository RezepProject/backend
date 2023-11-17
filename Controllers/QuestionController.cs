using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly DataContext _ctx;

        public QuestionController(DataContext ctx)
        {
            _ctx = ctx;
        }
    }
}