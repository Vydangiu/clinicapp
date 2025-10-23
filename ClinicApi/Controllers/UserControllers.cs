using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApi.Data.Models;

namespace ClinicApi.Controllers;
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ClinicDbContext _db;
    public UserController(ClinicDbContext db) => _db = db;

    // ===== Admin: Danh sách người dùng (kèm RoleName, IsAdmin) =====
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _db.Users
            .Include(u => u.Role)
            .Select(u => new
            {
                u.UserId,
                u.Username,
                RoleId = u.RoleId,
                RoleName = u.Role != null ? u.Role.RoleName : null,
                IsAdmin = u.Role != null && u.Role.IsAdmin,
                IsActive = u.IsActive ?? false,
                u.CreatedAt
            })
            .ToListAsync();

        return Ok(data);
    }

    // ===== Admin: Role options cho dropdown (hiển thị tất cả, có cờ IsAdmin) =====
    [Authorize(Roles = "Admin")]
    [HttpGet("role-options")]
    public async Task<IActionResult> GetRoleOptions()
    {
        var roles = await _db.Roles
            .Select(r => new { r.RoleId, r.RoleName, r.IsAdmin })
            .ToListAsync();

        return Ok(roles);
    }

    public record UpdateRoleDto(int RoleId);

    // ===== Admin: Cập nhật vai trò theo RoleId =====
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}/role")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "Không tìm thấy người dùng." });

        var role = await _db.Roles.FindAsync(dto.RoleId);
        if (role == null) return BadRequest(new { message = "Role không hợp lệ." });

        user.RoleId = role.RoleId;
        await _db.SaveChangesAsync();
        return Ok(new { message = "Cập nhật quyền thành công.", roleId = role.RoleId, role.RoleName });
    }

    // ===== Admin: Khóa/Mở tài khoản =====
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}/toggle")]
    public async Task<IActionResult> Toggle(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "Không tìm thấy người dùng." });

        user.IsActive = !(user.IsActive ?? false);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = (user.IsActive ?? false) ? "Đã mở khóa" : "Đã khóa tài khoản",
            isActive = user.IsActive ?? false
        });
    }

    // ===== Admin: Xóa người dùng =====
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound(new { message = "Không tìm thấy người dùng." });

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Đã xóa tài khoản." });
    }
}