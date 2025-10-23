using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class WorkSchedule
{
    public int ScheduleId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly WorkDate { get; set; }

    public string? Shift { get; set; }

    public string? Notes { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
