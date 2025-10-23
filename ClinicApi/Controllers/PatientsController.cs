using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApi.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClinicApi.Controllers;

[ApiController]
[Route("api/patients")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly ClinicDbContext _db;
    public PatientsController(ClinicDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.Patients.ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _db.Patients.FindAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Patient p)
    {
        _db.Patients.Add(p);
        await _db.SaveChangesAsync();
        return Ok(p);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Patient p)
    {
        var existing = await _db.Patients.FindAsync(id);
        if (existing == null) return NotFound();

        existing.FullName = p.FullName;
        existing.Gender = p.Gender;
        existing.DateOfBirth = p.DateOfBirth;
        existing.Phone = p.Phone;
        existing.Email = p.Email;
        existing.Address = p.Address;
        existing.GuardianName = p.GuardianName;
        existing.GuardianPhone = p.GuardianPhone;
        existing.HealthInsuranceNo = p.HealthInsuranceNo;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Patients.FindAsync(id);
        if (p == null) return NotFound();

        _db.Patients.Remove(p);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Đã xóa bệnh nhân." });
    }
}

