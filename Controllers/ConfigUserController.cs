using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]

[Route("[controller]")]
public class ConfigUserController : ControllerBase
{
    private readonly DataContext _context;

    public ConfigUserController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConfigUser>>> GetConfigUsers()
    {
        return await _context.ConfigUsers.ToListAsync();
    }
}