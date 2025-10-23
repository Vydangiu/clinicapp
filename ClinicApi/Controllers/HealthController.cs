using Microsoft.AspNetCore.Mvc;
using ClinicApi.Data.Models;

namespace ClinicApi.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly ClinicDbContext _db;
    public HealthController(ClinicDbContext db) => _db = db;

    [HttpGet("check")]
    public IActionResult Check()
    {
        bool ok = _db.Database.CanConnect();
        return Ok(new { connected = ok });
    }
}
