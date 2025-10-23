using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class VwPatientByMonth
{
    public int? Năm { get; set; }

    public int? Tháng { get; set; }

    public int? TổngBệnhNhânĐăngKýMới { get; set; }
}
