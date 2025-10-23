using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class VwExpensesByMonth
{
    public int? Năm { get; set; }

    public int? Tháng { get; set; }

    public decimal? TổngChiPhíVnđ { get; set; }
}
