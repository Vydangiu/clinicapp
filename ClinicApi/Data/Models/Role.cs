using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }
    public bool IsAdmin { get; set; } = false;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
     public virtual ICollection<User> Users { get; set; } = new List<User>();   
}
   
