using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class Medicine
{
    public int MedicineId { get; set; }

    public string MedicineName { get; set; } = null!;

    public string? Unit { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public string? Manufacturer { get; set; }

    public string? Usage { get; set; }

    public string? SideEffects { get; set; }

    public virtual ICollection<MedicineImport> MedicineImports { get; set; } = new List<MedicineImport>();

    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}
