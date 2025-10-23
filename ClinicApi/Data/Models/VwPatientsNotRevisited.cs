using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class VwPatientsNotRevisited
{
    public int PatientId { get; set; }

    public string TênBệnhNhân { get; set; } = null!;

    public DateOnly NgàyHẹnTáiKhám { get; set; }

    public int? SốNgàyTrễ { get; set; }

    public string? Notes { get; set; }
}
