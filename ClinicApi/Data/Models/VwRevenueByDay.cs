using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class VwRevenueByDay
{
    public DateOnly? NgàyThuTiền { get; set; }

    public decimal? TổngDoanhThuVnđ { get; set; }

    public int? SốPhiếuThu { get; set; }
}
