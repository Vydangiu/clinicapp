using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class VwTopMedicine
{
    public string TênThuốc { get; set; } = null!;

    public int? TổngSlBán { get; set; }

    public decimal? TổngDoanhThuVnđ { get; set; }
}
