using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int? EmployeeId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }  

    public virtual Employee? Employee { get; set; }


}
