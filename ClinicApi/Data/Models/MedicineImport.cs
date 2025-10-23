using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class MedicineImport
{
    public int ImportId { get; set; }

    public int MedicineId { get; set; }

    public DateTime? ImportDate { get; set; }

    public int Quantity { get; set; }

    public string? Supplier { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;
}
