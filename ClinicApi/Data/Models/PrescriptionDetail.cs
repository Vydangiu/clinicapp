using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class PrescriptionDetail
{
    public int PrescriptionDetailId { get; set; }

    public int PrescriptionId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }

    public string? Dosage { get; set; }

    public string? UsageInstruction { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual Prescription Prescription { get; set; } = null!;
}
