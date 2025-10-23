using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using BCrypt.Net;
using ClinicApi.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClinicApi.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ClinicDbContext _db;
    private readonly IConfiguration _cfg;

    public AuthController(ClinicDbContext db, IConfiguration cfg)
    {
        _db = db;
        _cfg = cfg;
    }

    private static bool IsStrongPassword(string password)
    {
        var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        return Regex.IsMatch(password, pattern);
    }

    public record RegisterDto(string Username, string Password, int RoleId);
    public record LoginDto(string Username, string Password);
    public record ChangePasswordDto(string OldPassword, string NewPassword);

    // ============= 1️⃣ Bootstrap admin đầu tiên (chỉ khi chưa có Admin nào) =============
    [AllowAnonymous]
    [HttpPost("bootstrap-admin")]
    public async Task<IActionResult> BootstrapAdmin([FromBody] RegisterDto dto)
    {
        bool hasAdmin = await _db.Users
            .Include(u => u.Role)
            .AnyAsync(u => u.Role != null && u.Role.IsAdmin);

        if (hasAdmin)
            return Conflict(new { message = "Hệ thống đã có Admin. Dùng tài khoản Admin để tạo người dùng mới." });
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { message = "Thiếu thông tin." });

        if (!IsStrongPassword(dto.Password))
            return BadRequest(new { message = "Mật khẩu phải >=8 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt." });

        var role = await _db.Roles.FindAsync(dto.RoleId);
        if (role == null || !role.IsAdmin)
            return BadRequest(new { message = "RoleId phải là role Admin hợp lệ." });

        if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            return Conflict(new { message = "Tên người dùng đã tồn tại." });

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            RoleId = role.RoleId
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Tạo Admin đầu tiên thành công.", username = user.Username });
    }

    // ============= 2️⃣ Admin tạo tài khoản người dùng mới =============
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { message = "Thiếu thông tin đăng ký." });

        if (!IsStrongPassword(dto.Password))
            return BadRequest(new { message = "Mật khẩu phải >=8 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt." });

        if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            return Conflict(new { message = "Tên người dùng đã tồn tại." });

        var role = await _db.Roles.FindAsync(dto.RoleId);
        if (role == null)
            return BadRequest(new { message = "RoleId không hợp lệ." });

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            RoleId = role.RoleId
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Tạo tài khoản thành công.", username = user.Username, role = role.RoleName });
    }

    // ============= 3️⃣ Đăng nhập =============
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { message = "Thiếu thông tin đăng nhập." });

        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });

        if (!(user.IsActive ?? false))
            return Forbid("Tài khoản đã bị khóa.");

        var accessRole = (user.Role?.IsAdmin ?? false) ? "Admin" : "User";
        var displayRole = user.Role?.RoleName ?? "User";

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, accessRole),
            new("displayRole", displayRole)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            access_token = jwt,
            username = user.Username,
            role = accessRole,
            displayRole,
            expires_in_hours = 6
        });
    }

    // ============= 4️⃣ Đổi mật khẩu =============
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(strId, out var userId))
            return Unauthorized(new { message = "Token không hợp lệ." });

        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound(new { message = "Không tìm thấy người dùng." });

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
            return BadRequest(new { message = "Mật khẩu cũ không đúng." });

        if (!IsStrongPassword(dto.NewPassword))
            return BadRequest(new { message = "Mật khẩu mới không đủ mạnh." });

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Đổi mật khẩu thành công." });
    }
    public record AdminResetPasswordDto(string NewPassword);
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/reset-password")]
    public async Task<IActionResult> AdminResetPassword(int id, [FromBody] AdminResetPasswordDto dto)
    {
        if (!IsStrongPassword(dto.NewPassword))
            return BadRequest(new { message = "Mật khẩu mới không đủ mạnh." });

        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound(new { message = "Không tìm thấy người dùng." });

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _db.SaveChangesAsync();
        return Ok(new { message = $"Đã đặt lại mật khẩu cho user #{id}." });
    }
    }